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
        }

        private void OnDisable()
        {
            minionManager.OnAllKilled -= Minions_AllDead;
            enemyManager.OnAllKilled  -= Enemies_AllDead;
            golem.Health.OnDeath      -= Golem_Died;
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
            // golem.StopBeam();

            yield return new WaitForSeconds(delay);

            while (golem.IsBeaming)
                yield return null;

            // go to black screen
            yield return sceneTransition.StartTransition();

            if (Keyboard.current?.fKey.isPressed == true)
                godModeEnabled = true;

            if (isNight)
            {
                dayNumber++;

                // loop back to day 1
                if (dayNumber > nightMapSO.NightCount)
                    dayNumber = 1;
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
            scrapManager.RespawnAll(Tonight.scrapPileAmount);
            minionManager.RespawnAll(PlayerState.minionAmount);

            enemyManager.RespawnAll(Tonight.GetEnemyAmount(waveIndex));
            enemyManager.SpawnMiniBosses(Tonight.GetMiniBossAmount(waveIndex));
        }

        private void Minions_AllDead()
        {
            // player can rely on golem abilities
        }

        private void Enemies_AllDead()
        {
            if (golem && golem.Health && !golem.Health.IsDead)
            {
                bool nightEnded = !NextWave();

                if (nightEnded)
                    AudioPlayer.Instance.uiNightSuccess.PlayOneShot();
            }
        }

        private void Golem_Died(Health source)
        {
            AudioPlayer.Instance.uiGameOver.PlayOneShot();
            Invoke(nameof(ResetGame), Config.GameRestartTime);
        }

        private void ResetGame()
        {
            isNight   = false;
            dayNumber = 1;
            waveIndex = 0;

            PlayerState.Reset();

            golem.Health.gameObject.SetActive(true);
            golem.Health.Revive();

            UpdateTimeScale();
            OnPhaseChange?.Invoke();
        }

        private bool NextWave()
        {
            waveIndex++;

            if (waveIndex >= Tonight.Waves.Length)
            {
                StartCoroutine(NextPhase(gameConfigSO.NightWaitTime));
                return false;
            }

            EnemyManager.Spawn(Tonight.GetEnemyAmount(waveIndex));
            return true;
        }

        private void UpdateTimeScale() =>
            Time.timeScale = isNight ? 1f : 0f;
    }
}
