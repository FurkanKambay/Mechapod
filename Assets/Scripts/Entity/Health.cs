using System;
using UnityEngine;

namespace Crabgame.Entity
{
    public class Health : MonoBehaviour
    {
        public event Action OnHurt;
        public event Action OnDeath;

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

            OnHurt?.Invoke();

            if (health <= 0)
                Die();
        }

        public virtual void Die()
        {
            health = 0;

            enabled = false;
            OnDeath?.Invoke();
        }
    }
}
