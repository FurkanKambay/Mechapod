using UnityEngine;

namespace Crabgame.Entity
{
    public class Health : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int maxHealth;

        [Header("Debug")]
        [SerializeField] private int health;

        public int MaxHealth => maxHealth;

        public int CurrentHealth => health;

        public bool IsDead => health <= 0;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health <= 0)
                Die();
        }

        public void Die()
        {
            health = 0;

            // TODO: particles
            Destroy(gameObject);
        }
    }
}
