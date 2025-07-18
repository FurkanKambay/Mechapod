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

        private bool isArmBeamUsedUp;

        // ARM BEAM
        public bool  IsBeaming { get; private set; }
        public float BeamAngle { get; private set; }

        private float targetBeamAngle;
        private float beamDamageCountdown;

        private Collider2D[] beamResults = new Collider2D[5];
        //

        private void Awake()
        {
            ResetAbilities();
        }

        private void Update()
        {
            AimPoint = input.AimPosition;

            if (IsBeaming)
                TickArmBeam();
            else if (isArmBeamUsedUp && GameManager.PlayerState.GolemHasArm && input.WantsToUseArm)
                UseArm();
        }

        public void StartBeam() => IsBeaming = true;
        public void StopBeam()  => IsBeaming = true;

        private GameConfigSO Config => GameManager.Config;

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

            Vector3 center   = beamOrigin.position + realAimDirection * Config.BeamLength / 2;
            var     boxSize  = new Vector2(GameManager.Config.BeamLength, Config.BeamWidth);
            float   boxAngle = BeamAngle;

            int hitCount = Physics2D.OverlapBox(
                point: center,
                boxSize,
                boxAngle,
                beamAttackFilter,
                beamResults
            );

            if (hitCount <= 0)
                return;

            Debug.Log($"Beam hits {hitCount}, first: {beamResults[0].name}");

            for (var i = 0; i < hitCount; i++)
            {
                var target = beamResults[i].GetComponent<Health>();
                target.TakeDamage(Config.BeamDamage);
            }
        }

        private void UseArm()
        {
            isArmBeamUsedUp = false;
            OnGolemArm?.Invoke();
        }

        public void ResetAbilities()
        {
            isArmBeamUsedUp = true;
        }
    }
}
