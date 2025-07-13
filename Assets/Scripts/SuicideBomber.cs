using UnityEngine;

namespace Shovel
{
    public class SuicideBomber : MonoBehaviour
    {
        [SerializeField] private int damageAmount;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Player"))
                return;

            var golemHealth = other.collider.GetComponent<Health>();
            golemHealth.health -= damageAmount;

            print("explode");
        }
    }
}
