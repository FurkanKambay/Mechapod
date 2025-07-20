using System;
using Crabgame.Entity;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Player
{
    public class Golem : MonoBehaviour
    {
        public event Action OnArmBeamTriggered;
        public event Action OnArmBeamStarted;
        public event Action OnArmBeamStopped;

        [Header("Input")]
        [SerializeField] private PlayerInput input;

        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private Transform beamOrigin;

        [Header("Config - Beam")]
        [SerializeField] private ContactFilter2D beamAttackFilter;

        [Range(-360f, 360f)]
        [SerializeField] private float beamMinAngle = 25f;

        [Range(-360f, 360f)]
        [SerializeField] private float beamMaxAngle = -140f;

        public Health Health => health;

        public float BeamMinAngle => beamMinAngle;
        public float BeamMaxAngle => beamMaxAngle;

        public Vector2 AimPoint { get; private set; }

        // ARM BEAM
        public bool  IsArmBeamAvailable { get; private set; }
        public bool  IsBeaming          { get; private set; }
        public float BeamAngle          { get; private set; }

        private float targetBeamAngle;
        private float beamDamageCountdown;

        private readonly Collider2D[] beamResults = new Collider2D[5];
        //

        private static GameConfigSO Config => GameManager.Config;

        private void Awake()
        {
            ResetAbilities();
        }

        private void Update()
        {
            AimPoint = input.AimPosition;

            if (IsBeaming)
                TickArmBeam();
            else if (IsArmBeamAvailable && GameManager.PlayerState.GolemHasArm && input.WantsToUseArm)
                UseArm();
        }

        public void StartBeam()
        {
            IsBeaming = true;
            OnArmBeamStarted?.Invoke();
        }

        public void StopBeam()
        {
            IsBeaming = false;
            OnArmBeamStopped?.Invoke();
        }

        private void TickArmBeam()
        {
            Vector2 direction = AimPoint - (Vector2)beamOrigin.position;
            targetBeamAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetBeamAngle = Mathf.Clamp(targetBeamAngle, BeamMinAngle, BeamMaxAngle);

            BeamAngle = Mathf.MoveTowardsAngle(BeamAngle, targetBeamAngle, Config.BeamFollowSpeed * Time.deltaTime);

            float radians          = Mathf.Deg2Rad * BeamAngle;
            var   realAimDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            DamageTargets(realAimDirection);
        }

        private void DamageTargets(Vector3 realAimDirection)
        {
            Vector3 beamVector = realAimDirection * Config.BeamLength;
            Debug.DrawRay(beamOrigin.position, beamVector, Color.red);

            beamDamageCountdown -= Time.deltaTime;

            if (beamDamageCountdown > 0)
                return;

            beamDamageCountdown = Config.BeamDamageRate;

            Vector3 center  = beamOrigin.position + realAimDirection * Config.BeamLength / 2;
            var     boxSize = new Vector2(GameManager.Config.BeamLength, Config.BeamWidth);

            int hitCount = Physics2D.OverlapBox(
                point: center,
                boxSize,
                BeamAngle,
                beamAttackFilter,
                beamResults
            );

            if (hitCount <= 0)
                return;

            for (var i = 0; i < hitCount; i++)
            {
                var target = beamResults[i].GetComponent<Health>();
                target.TakeDamage(Config.BeamDamage, health);
            }
        }

        private void UseArm()
        {
            IsArmBeamAvailable = false;
            OnArmBeamTriggered?.Invoke();
        }

        public void ResetAbilities()
        {
            IsArmBeamAvailable = true;
        }
    }
}
