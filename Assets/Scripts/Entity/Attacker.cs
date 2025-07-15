using System.Collections;
using Shovel.Audio;
using UnityEngine;

namespace Shovel.Entity
{
    public class Attacker : MonoBehaviour
    {
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
            Attack();
        }

        public void Attack()
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, attackRadius, attackLayers);

            AudioPlayer.Instance.crabAttack.PlayOneShot();

            if (!target)
            {
                // AudioPlayer.Instance.crabAttackMiss.PlayOneShot();
                return;
            }

            // AudioPlayer.Instance.crabAttack.PlayOneShot();

            var targetHealth = target.GetComponent<Health>();
            targetHealth.health -= damage;
        }
    }
}
