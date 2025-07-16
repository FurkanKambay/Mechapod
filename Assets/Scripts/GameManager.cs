using System;
using Shovel.Night;
using Shovel.Player;
using UnityEngine;

namespace Shovel
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnPhaseChange;

        [Header("References")]
        [SerializeField] private GameConfigSO gameConfigSO;
        [SerializeField] private NightMapSO nightMapSO;

        [Header("State - Game")]
        [SerializeField, Min(1)] private int dayNumber = 1;
        [SerializeField] private bool isNight;

        [Header("State - Player & Enemy")]
        [SerializeField] private PlayerState playerState;

        public static GameConfigSO Config  => Instance.gameConfigSO;
        public static PlayerState  PlayerState => Instance.playerState;
        public static NightInfo    Tonight     => Instance.nightMapSO.Nights[Instance.dayNumber - 1];

        public int  DayNumber => dayNumber;
        public bool IsNight   => isNight;

        private void Awake()
        {
            Instance = this;

            dayNumber = 1;
            isNight   = false;

            UpdateTimeScale();
        }

        [ContextMenu("Next Phase (Day/Night)")]
        public void NextPhase()
        {
            if (isNight)
                dayNumber++;

            isNight = !isNight;

            UpdateTimeScale();
            OnPhaseChange?.Invoke();
        }

        private void UpdateTimeScale() =>
            Time.timeScale = isNight ? 1f : 0f;
    }
}
