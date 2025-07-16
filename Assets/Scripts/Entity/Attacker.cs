using System;
using UnityEngine;

namespace Shovel.Entity
{
    public class Attacker : MonoBehaviour
    {
        public event Action       OnAttackPerformed;
        public event Action<bool> OnAttackProcced;

        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Collider2D attackCollider;
        [SerializeField] private Collider2D detectionTrigger;

        [Header("Config - Attack")]
        [SerializeField] private ContactFilter2D attackFilter;

        [SerializeField] private int   damage;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackRate;

        [Header("Config - Detection")]
        [SerializeField] private float detectionRange;

        public Rigidbody2D Body => body;

        [Header("State")]
        [SerializeField] private Vector2 velocity;
        [SerializeField] private Direction aimDirection;
        [SerializeField] public  Direction lockedDirection;

        [SerializeField] public  float attackOffset;
        [SerializeField] private float attackTimer;

        [SerializeField] public bool isPerformingAttack;
        [SerializeField] public bool isRecovering;

        public Direction AimDirection => aimDirection;

        private Transform    attackBox;
        private Collider2D[] attackResults = new Collider2D[5];

        private Transform    detectionBox;
        private Collider2D[] detectionResults = new Collider2D[1];

        private bool shouldAttack;

        private void Start()
        {
            attackTimer  = -attackOffset;
            attackBox    = attackCollider.transform;
            detectionBox = detectionTrigger.transform;

            Vector3 attackScale = attackBox.localScale;
            attackScale.x        = attackRange;
            attackBox.localScale = attackScale;

            Vector3 detectionScale = detectionBox.localScale;
            detectionScale.x        = detectionRange;
            detectionBox.localScale = detectionScale;
        }

        private void Update()
        {
            UpdateAimDirection();

            attackTimer += Time.deltaTime;

            if (attackTimer < attackRate)
                return;

            CheckShouldAttack();

            if (shouldAttack)
                PerformAttack();
        }

        private void UpdateAimDirection()
        {
            velocity = body.linearVelocity;

            aimDirection = (Math.Sign(velocity.x), Math.Sign(velocity.y)) switch
            {
                (-1, 1)                  => Direction.NorthWest,
                (1, 1)                   => Direction.NorthEast,
                (1, -1)                  => Direction.SouthEast,
                (-1, -1)                 => Direction.SouthWest,
                _ when aimDirection == 0 => Direction.SouthWest,
                _                        => aimDirection
            };
        }

        private void CheckShouldAttack()
        {
            if (shouldAttack)
                return;

            detectionResults = new Collider2D[1];

            detectionBox.eulerAngles = Vector3.forward * lockedDirection.AttackAngle();
            int hitCount = detectionTrigger.Overlap(attackFilter, detectionResults);

            shouldAttack = hitCount > 0;
        }

        private void PerformAttack()
        {
            attackTimer  = 0;
            shouldAttack = false;

            isPerformingAttack = true;
            OnAttackPerformed?.Invoke();
        }

        internal bool ProcAttack()
        {
            attackResults = new Collider2D[5];

            attackBox.eulerAngles = Vector3.forward * lockedDirection.AttackAngle();
            int hitCount = attackCollider.Overlap(attackFilter, attackResults);

            isPerformingAttack = false;
            isRecovering       = true;
            OnAttackProcced?.Invoke(hitCount != 0);

            if (hitCount == 0)
                return false;

            for (var i = 0; i < hitCount; i++)
            {
                var target = attackResults[i].GetComponent<Health>();
                target.TakeDamage(damage);
            }

            return true;
        }
    }
}
