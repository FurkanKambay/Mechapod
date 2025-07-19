using System;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Entity
{
    public class Attacker : MonoBehaviour
    {
        public event Action       OnAttackPerformed;
        public event Action<bool> OnAttackProcced;

        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private Rigidbody2D  body;
        [SerializeField] private Collider2D[] attackTriggers;
        [SerializeField] private Collider2D[] detectionTriggers;

        [Header("Config - Special")]
        [SerializeField] private float speedMultiplier = 1;

        [Header("Config - Attack")]
        [SerializeField] private ContactFilter2D attackFilter;

        [SerializeField] private int   damage;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackRate;

        [Header("Config - Detection")]
        [SerializeField] private float detectionRange;

        public Health      Health => health;
        public Rigidbody2D Body   => body;

        public Direction AimDirection    => aimDirection ?? moveDirection;
        public float     SpeedMultiplier => speedMultiplier;
        public bool      TurningBlocked  { get; private set; }

        [Header("State")]
        [SerializeField] private Vector2 velocity;
        [SerializeField] private Direction moveDirection;
        [SerializeField] private Direction lockedDirection;

        [SerializeField] public  float attackOffset;
        [SerializeField] private float attackTimer;

        [SerializeField] public bool isPerformingAttack;
        [SerializeField] public bool isRecovering;

        private SpawnerManager sourceSpawner;
        private Direction?     aimDirection;
        private bool           shouldAttack;

        private          Transform[]  attackBoxes;
        private          Transform[]  detectionBoxes;
        private readonly Collider2D[] attackResults    = new Collider2D[5];
        private readonly Collider2D[] detectionResults = new Collider2D[1];

        private void Awake()
        {
            InitTriggers();
            attackTimer     = -attackOffset;
            lockedDirection = Direction.SouthEast;
        }

        private void OnEnable()  => health.OnDeath += Health_Death;
        private void OnDisable() => health.OnDeath -= Health_Death;

        private void Update()
        {
            if (health.IsDead)
            {
                enabled = false;
                return;
            }

            UpdateAimDirection();

            attackTimer += Time.deltaTime;

            if (attackTimer < attackRate)
                return;

            shouldAttack = ShouldAttack();

            if (shouldAttack)
                PerformAttack();
        }

        private void Health_Death()
        {
            if (sourceSpawner)
                sourceSpawner.Despawn(this);
        }

        internal void RegisterSpawner(SpawnerManager spawner) =>
            sourceSpawner = spawner;

        private void UpdateAimDirection()
        {
            velocity = body.linearVelocity;

            moveDirection = (Math.Sign(velocity.x), Math.Sign(velocity.y)) switch
            {
                (-1, 1)                   => Direction.NorthWest,
                (1, 1)                    => Direction.NorthEast,
                (1, -1)                   => Direction.SouthEast,
                (-1, -1)                  => Direction.SouthWest,
                _ when moveDirection == 0 => Direction.SouthWest,
                _                         => moveDirection
            };

            TurningBlocked = (isPerformingAttack && !GameManager.Config.TurnWhileAttacking)
                             || (isRecovering && !GameManager.Config.TurnWhileRecovering);

            if (!TurningBlocked)
                lockedDirection = AimDirection;
        }

        private bool ShouldAttack()
        {
            if (shouldAttack)
                return true;

            int hitCount = GetDetections(lockedDirection);

            if (hitCount > 0)
                return true;

            var angles = new[] { Direction.NorthWest, Direction.NorthEast, Direction.SouthEast, Direction.SouthWest };

            foreach (Direction direction in angles)
            {
                if (direction == lockedDirection)
                    continue;

                hitCount = GetDetections(direction);

                if (hitCount == 0)
                    continue;

                aimDirection = direction;
                return true;
            }

            aimDirection = null;
            return false;
        }

        private int GetDetections(Direction direction)
        {
            Collider2D trigger = detectionTriggers[(int)direction - 1];
            return trigger.Overlap(attackFilter, detectionResults);
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
            isPerformingAttack = false;
            isRecovering       = true;

            Collider2D trigger  = attackTriggers[(int)lockedDirection - 1];
            int        hitCount = trigger.Overlap(attackFilter, attackResults);

            OnAttackProcced?.Invoke(hitCount != 0);

            if (hitCount == 0)
                return false;

            for (var i = 0; i < hitCount; i++)
            {
                if (attackResults[i] && attackResults[i].TryGetComponent(out Health target))
                    target.TakeDamage(damage);
            }

            return true;
        }

        private void InitTriggers()
        {
            Array.Resize(ref attackBoxes,    attackTriggers.Length);
            Array.Resize(ref detectionBoxes, detectionTriggers.Length);

            for (var i = 0; i < attackTriggers.Length; i++)
                attackBoxes[i] = attackTriggers[i].transform;

            for (var i = 0; i < detectionTriggers.Length; i++)
                detectionBoxes[i] = detectionTriggers[i].transform;
        }

        private void OnValidate()
        {
            InitTriggers();

            foreach (Transform box in attackBoxes)
            {
                Vector3 attackScale = box.localScale;
                attackScale.x  = attackRange;
                box.localScale = attackScale;
            }

            foreach (Transform box in detectionBoxes)
            {
                Vector3 detectionScale = box.localScale;
                detectionScale.x = detectionRange;
                box.localScale   = detectionScale;
            }
        }
    }
}
