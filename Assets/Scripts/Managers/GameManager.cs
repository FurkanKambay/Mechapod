using System;
using Crabgame.Entity;
using Crabgame.Night;
using Crabgame.Player;
using UnityEngine;

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
        [SerializeField] private Health       golemHealth;

        [Header("State - Game")]
        [SerializeField] private bool isNight;
        [SerializeField, Min(1)] private int dayNumber = 1;
        [SerializeField, Min(0)] private int waveIndex;

        [Header("State - Player & Enemy")]
        [SerializeField] private PlayerState playerState;

        public static GameConfigSO  Config        => Instance.gameConfigSO;
        public static MinionManager MinionManager => Instance.minionManager;
        public static EnemyManager  EnemyManager  => Instance.enemyManager;
        public static PlayerState   PlayerState   => Instance.playerState;
        public static NightInfo     Tonight       => Instance.nightMapSO.GetNight(Instance.dayNumber - 1);

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
        }

        private void OnDisable()
        {
            minionManager.OnAllKilled -= Minions_AllDead;
            enemyManager.OnAllKilled  -= Enemies_AllDead;
        }

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
                PopulateTonight();
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
            MinionManager.RespawnAll(PlayerState.minionAmount);
            EnemyManager.RespawnAll(Tonight.GetEnemyAmount(waveIndex));

            // TODO: respawn Scrap Piles
        }

        private void Minions_AllDead()
        {
            // player can rely on golem abilities
        }

        private void Enemies_AllDead()
        {
            NextWave();
        }

        public void NextWave()
        {
            waveIndex++;

            if (waveIndex >= Tonight.Waves.Length)
            {
                Invoke(nameof(NextPhase), time: gameConfigSO.NightWaitTime);
                // NextPhase();
                return;
            }

            EnemyManager.Spawn(Tonight.GetEnemyAmount(waveIndex));
        }

        private void UpdateTimeScale() =>
            Time.timeScale = isNight ? 1f : 0f;
    }
}
