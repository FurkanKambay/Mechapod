using System;
using System.Collections;
using Crabgame.Audio;
using Crabgame.Entity;
using Crabgame.Night;
using Crabgame.Player;
using Crabgame.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crabgame.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnPhaseChange;

        [Header("References - Assets")]
        [SerializeField] private GameConfigSO gameConfigSO;
        [SerializeField] private NightMapSO nightMapSO;

        // used in HUD.uxml
        [Header("References - Scene")]
        [SerializeField] private MinionManager minionManager;
        [SerializeField] private EnemyManager    enemyManager;
        [SerializeField] private ScrapManager    scrapManager;
        [SerializeField] private Golem           golem;
        [SerializeField] private SceneTransition sceneTransition;

        [Header("State - Game")]
        [SerializeField] private bool isNight;
        [SerializeField, Min(1)] private int dayNumber = 1;
        [SerializeField, Min(0)] private int waveIndex;

        [Header("State - Player & Enemy")]
        [SerializeField] private PlayerState playerState;

        public static GameConfigSO Config => Instance.gameConfigSO;

        public static MinionManager MinionManager => Instance.minionManager;
        public static EnemyManager  EnemyManager  => Instance.enemyManager;
        public static ScrapManager  ScrapManager  => Instance.scrapManager;
        public static Golem         Golem         => Instance.golem;

        public static PlayerState PlayerState => Instance.playerState;
        public static NightInfo   Tonight     => Instance.nightMapSO.GetNight(Instance.dayNumber - 1);

        public int  DayNumber => dayNumber;
        public bool IsNight   => isNight;

        private bool godModeEnabled;
        private bool playerMinionsAllDead;

        private void Awake()
        {
            Instance = this;

            isNight   = false;
            dayNumber = 1;
            waveIndex = 0;

            UpdateTimeScale();
        }

        private void OnEnable()
        {
            minionManager.OnAllKilled += Minions_AllDead;
            enemyManager.OnAllKilled  += Enemies_AllDead;
            golem.Health.OnDeath      += Golem_Died;
            golem.OnArmBeamStopped    += Golem_ArmBeamStopped;
        }

        private void OnDisable()
        {
            minionManager.OnAllKilled -= Minions_AllDead;
            enemyManager.OnAllKilled  -= Enemies_AllDead;
            golem.Health.OnDeath      -= Golem_Died;
            golem.OnArmBeamStopped    -= Golem_ArmBeamStopped;
        }

        private void Update()
        {
#if !UNITY_EDITOR
            if (!godModeEnabled)
                return;
#endif

            Keyboard keyboard = Keyboard.current;

            if (keyboard.rightBracketKey.wasPressedThisFrame || keyboard.numpadPlusKey.wasPressedThisFrame)
                StartCoroutine(NextPhase());
            else if (keyboard.leftBracketKey.wasPressedThisFrame || keyboard.numpadMinusKey.wasPressedThisFrame)
                NextWave();

            // KILL ALL CRABS
            if (keyboard.jKey.wasPressedThisFrame)
                minionManager.Clear();
            else if (keyboard.kKey.wasPressedThisFrame)
                enemyManager.Clear();

            if (keyboard.nKey.wasPressedThisFrame)
                enemyManager.Spawn(1);
            else if (keyboard.mKey.wasPressedThisFrame)
                minionManager.Spawn(1);
            else if (keyboard.bKey.wasPressedThisFrame)
                enemyManager.SpawnMiniBosses(1);

            if (keyboard.aKey.wasPressedThisFrame)
                playerState.GiveArm();
        }

        [ContextMenu("Next Phase (Day/Night)")]
        public IEnumerator NextPhase(float delay = 0)
        {
            yield return new WaitForSecondsRealtime(delay);

            golem.StopBeam();

            if (Keyboard.current?.fKey.isPressed == true)
            {
                Debug.Log("[Mechapod] Godmode enabled!");
                godModeEnabled = true;
            }

            // go to black screen
            yield return sceneTransition.StartTransition();

            if (isNight)
            {
                dayNumber++;

                // loop back to day 1
                if (dayNumber > nightMapSO.NightCount)
                {
                    dayNumber = 1;
                    yield return GameSuccess();
                    yield break;
                }
            }

            isNight   = !isNight;
            waveIndex = 0;

            if (isNight)
            {
                golem.ResetAbilities();
                PopulateTonight();
            }
            else
            {
                minionManager.Clear();
                enemyManager.Clear();
            }

            UpdateTimeScale();
            OnPhaseChange?.Invoke();

            // hide black screen
            yield return sceneTransition.EndTransition();
        }

        private void PopulateTonight()
        {
            playerMinionsAllDead = false;
            minionManager.RespawnAll(PlayerState.minionAmount);
            scrapManager.RespawnAll(Tonight.scrapPileAmount);

            enemyManager.SpawnWave(Tonight.Waves[waveIndex]);
        }

        private void Golem_ArmBeamStopped() =>
            SpeedUpGame_IfNoActionAvailable();

        private void Minions_AllDead()
        {
            playerMinionsAllDead = true;
            SpeedUpGame_IfNoActionAvailable();
        }

        private void SpeedUpGame_IfNoActionAvailable()
        {
            if (!golem || !golem.Health || golem.Health.IsDead)
                return;

            if (playerMinionsAllDead && !golem.CanLaunchArmBeam)
                Time.timeScale = 3;
        }

        private void Enemies_AllDead()
        {
            if (golem && golem.Health && !golem.Health.IsDead)
                NextWave();
        }

        private void Golem_Died(Health source)
        {
            Time.timeScale = 1;
            StartCoroutine(GameFail());
        }

        private IEnumerator GameSuccess()
        {
            // AudioPlayer.Instance.uiGameSuccess.PlayOneShot();
            ClearEntities();

            sceneTransition.GameSuccess(true);
            yield return new WaitForSecondsRealtime(Config.GameRestartTime);

            sceneTransition.GameSuccess(false);
            ResetGame();
        }

        private IEnumerator GameFail()
        {
            ClearEntities();

            yield return new WaitForSecondsRealtime(Config.GameRestartTime / 2f);

            AudioPlayer.Instance.uiGameOver.PlayOneShot();

            sceneTransition.GameOver(true);
            yield return new WaitForSecondsRealtime(Config.GameRestartTime);
            sceneTransition.GameOver(false);

            ResetGame();
        }

        private void ClearEntities()
        {
            golem.enabled = false;

            minionManager.Clear();
            enemyManager.Clear();
            scrapManager.Clear();
        }

        private void ResetGame()
        {
            ClearEntities();

            isNight   = false;
            dayNumber = 1;
            waveIndex = 0;

            PlayerState.Reset();

            golem.enabled = true;
            golem.Health.gameObject.SetActive(true);
            golem.Health.Revive();

            UpdateTimeScale();
            OnPhaseChange?.Invoke();
        }

        private void NextWave()
        {
            waveIndex++;

            bool isDayFinished = waveIndex >= Tonight.Waves.Length;

            if (isDayFinished)
                StartCoroutine(WaveFinished());
            else
                enemyManager.SpawnWave(Tonight.Waves[waveIndex]);
        }

        private IEnumerator WaveFinished()
        {
            yield return new WaitForSecondsRealtime(gameConfigSO.NightWaitTime / 2f);

            AudioPlayer.Instance.uiNightSuccess.PlayOneShot();

            yield return NextPhase(gameConfigSO.NightWaitTime / 2f);
        }

        private void UpdateTimeScale() =>
            Time.timeScale = isNight ? 1f : 0f;
    }
}
