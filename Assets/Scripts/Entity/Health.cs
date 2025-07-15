using UnityEngine;

namespace Shovel.Entity
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;

        public int health;

        public int MaxHealth => maxHealth;

        private void Awake()
        {
            health = maxHealth;
        }
    }
}
