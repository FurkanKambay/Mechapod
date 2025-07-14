using System;
using UnityEngine;

namespace Shovel
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnPhaseChange;

        [Header("References")]
        [SerializeField] private MinionManager minionManager;
        [SerializeField] private Health golemHealth;

        [Header("State")]
        [SerializeField, Min(1)] private int dayNumber = 1;
        [SerializeField] private bool isNight;

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
