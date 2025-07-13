using UnityEngine;

namespace Shovel
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        public int health;

        private void Awake()
        {
            health = maxHealth;
        }
    }
}
