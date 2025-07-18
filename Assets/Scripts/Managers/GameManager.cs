using System;
using Crabgame.Entity;
using Crabgame.Night;
using Crabgame.Player;
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
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private ScrapManager scrapManager;
        [SerializeField] private Golem       golem;

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

        public static PlayerState PlayerState => Instance.playerState;
        public static NightInfo   Tonight     => Instance.nightMapSO.GetNight(Instance.dayNumber - 1);

        public int  DayNumber => dayNumber;
        public bool IsNight   => isNight;

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
            golem.Health.OnDeath       += Golem_Died;
        }

        private void OnDisable()
        {
            minionManager.OnAllKilled -= Minions_AllDead;
            enemyManager.OnAllKilled  -= Enemies_AllDead;
            golem.Health.OnDeath      -= Golem_Died;
        }

#if UNITY_EDITOR
        private void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard.equalsKey.wasPressedThisFrame || keyboard.numpadPlusKey.wasPressedThisFrame)
                NextPhase();

            if (keyboard.jKey.wasPressedThisFrame)
                minionManager.Clear();

            if (keyboard.kKey.wasPressedThisFrame)
                enemyManager.Clear();

            if (keyboard.bKey.wasPressedThisFrame)
                enemyManager.SpawnMiniBoss();
        }
#endif

        [ContextMenu("Next Phase (Day/Night)")]
        public void NextPhase()
        {
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
        }

        private void PopulateTonight()
        {
            scrapManager.RespawnAll(Tonight.scrapPileAmount);
            minionManager.RespawnAll(PlayerState.minionAmount);

            enemyManager.RespawnAll(Tonight.GetEnemyAmount(waveIndex));

            if (nightMapSO.MiniBossNight == dayNumber)
                enemyManager.SpawnMiniBoss();
        }

        private void Minions_AllDead()
        {
            // player can rely on golem abilities
        }

        private void Enemies_AllDead()
        {
            if (golem && golem.Health && !golem.Health.IsDead)
                NextWave();
        }

        private void Golem_Died()
        {
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

        private void NextWave()
        {
            waveIndex++;

            if (waveIndex >= Tonight.Waves.Length)
            {
                Invoke(nameof(NextPhase), time: gameConfigSO.NightWaitTime);
                return;
            }

            EnemyManager.Spawn(Tonight.GetEnemyAmount(waveIndex));
        }

        private void UpdateTimeScale() =>
            Time.timeScale = isNight ? 1f : 0f;
    }
}
