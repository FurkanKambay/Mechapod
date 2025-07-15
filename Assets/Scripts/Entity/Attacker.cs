using System;
using Shovel.Audio;
using UnityEngine;

namespace Shovel.Entity
{
    public class Attacker : MonoBehaviour
    {
        public event Action OnAttackPerformed;
        public event Action<bool> OnAttackProcced;

        [Header("References")]
        [SerializeField] private Rigidbody2D body;

        [Header("Config")]
        [SerializeField] private LayerMask attackLayers;
        [SerializeField] private int   damage;
        [SerializeField] private float attackRadius;
        [SerializeField] private float attackRate;

        public float attackOffset;

        public Rigidbody2D Body => body;

        public float AttackRadius => attackRadius;

        [Header("State")]
        [SerializeField] private float attackTimer;

        private void Start()
        {
            attackTimer = -attackOffset;
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

        internal bool ProcAttack()
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, attackRadius, attackLayers);
            OnAttackProcced?.Invoke((bool)target);

            if (!target)
                return false;

            var targetHealth = target.GetComponent<Health>();
            targetHealth.TakeDamage(damage);
            return true;
        }
    }
}
