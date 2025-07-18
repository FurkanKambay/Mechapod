using System;
using Crabgame.Audio;
using UnityEngine;

namespace Crabgame.Entity
{
    public class Health : MonoBehaviour
    {
        public event Action OnHurt;
        public event Action OnDeath;

        [Header("Config")]
        [SerializeField] private EntityType entityType;
        [SerializeField] private int maxHealth;

        [Header("Debug")]
        [SerializeField] private int health;

        public EntityType EntityType => entityType;

        public int  MaxHealth     => maxHealth;
        public int  CurrentHealth => health;
        public bool IsDead        => health <= 0;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health > 0)
            {
                AudioPlayer.Instance.PlayEntityGetHurt(entityType);
                OnHurt?.Invoke();
            }
            else
                Die();
        }

        public virtual void Die()
        {
            health = 0;

            AudioPlayer.Instance.PlayEntityDeath(entityType);

            enabled = false;
            OnDeath?.Invoke();
        }

        public void Revive()
        {
            enabled = true;
            health  = maxHealth;
        }
    }
}
