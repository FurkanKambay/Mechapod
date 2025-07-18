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

        [Header("Config")]
        [SerializeField, Min(10f)] private float beamFollowSpeed = 2f;

        [Range(-360f, 360f)]
        [SerializeField] private float beamMinAngle = 25f;

        [Range(-360f, 360f)]
        [SerializeField] private float beamMaxAngle = -140f;

        public Health Health => health;

        public float BeamFollowSpeed => beamFollowSpeed;
        public float BeamMinAngle    => beamMinAngle;
        public float BeamMaxAngle    => beamMaxAngle;

        public Vector2 AimPoint { get; private set; }

        private bool canUseArm;

        private void Awake()
        {
            ResetAbilities();
        }

        private void Update()
        {
            AimPoint = input.AimPosition;

            bool hasArm = GameManager.PlayerState.GolemHasArm;

            if (!hasArm || !canUseArm || !input.WantsToUseArm)
                return;

            UseArm();
            canUseArm = false;
        }

        public void DamageTargets(Vector3 realAimDirection)
        {
            Vector3 vector = realAimDirection * GameManager.Config.BeamLength;
            // TODO: raycast, damage things

            // maybe limit the damage RATE? (cant one-shot boss)
        }

        private void UseArm()
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
