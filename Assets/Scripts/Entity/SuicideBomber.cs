using Shovel.Audio;
using UnityEngine;

namespace Shovel.Entity
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

            AudioPlayer.Instance.crabEnemyExplode.PlayOneShot();

            // TODO: particles
            Destroy(gameObject);
        }
    }
}
