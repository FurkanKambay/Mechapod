using System;
using Crabgame.Entity;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Player
{
    public class Golem : MonoBehaviour
    {
        public event Action OnGolemArm;

        [Header("Input")]
        [SerializeField] private PlayerInput input;

        [Header("References")]
        [SerializeField] private Health health;

        public Health Health => health;

        private bool canUseArm;

        private void Awake()
        {
            ResetAbilities();
        }

        private void Update()
        {
            bool hasArm = GameManager.PlayerState.GolemHasArm;

            if (!hasArm || !canUseArm || !input.WantsToUseArm)
                return;

            UseArm();
            canUseArm = false;
        }

        public void UseArm()
        {
            OnGolemArm?.Invoke();
            canUseArm = false;
        }

        public void ResetAbilities()
        {
            canUseArm = true;
        }
    }
}
