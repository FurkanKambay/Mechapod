using System;
using UnityEditor;
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

        [Header("Config")]
        [SerializeField] private ContactFilter2D attackFilter;

        [SerializeField] private int   damage;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackRate;

        public Rigidbody2D Body        => body;
        public float       AttackRange => attackRange;

        [Header("State")]
        public float attackOffset;
        [SerializeField] private float attackTimer;

        private Transform attackBox;

        private void Start()
        {
            attackTimer = -attackOffset;
            attackBox   = attackCollider.transform;

            Vector3 attackScale = attackBox.localScale;
            attackScale.x        = attackRange;
            attackBox.localScale = attackScale;
        }

        private void Update()
        {
            attackTimer += Time.deltaTime;

            if (attackTimer < attackRate)
                return;

            attackTimer = 0;
            PerformAttack();
        }

        private void PerformAttack()
        {
            OnAttackPerformed?.Invoke();
        }

        internal bool ProcAttack(Direction direction)
        {
            var results = new Collider2D[5];

            attackBox.eulerAngles = Vector3.forward * direction.AttackAngle();
            int hitCount = attackCollider.Overlap(attackFilter, results);

            OnAttackProcced?.Invoke(hitCount != 0);

            if (hitCount == 0)
                return false;

            for (var i = 0; i < hitCount; i++)
            {
                var target = results[i].GetComponent<Health>();
                target.TakeDamage(damage);
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position;

            Handles.color = Color.green;
            // Handles.DrawWireDisc(origin, Vector3.forward, AttackRange);
        }
    }
}
