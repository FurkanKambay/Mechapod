using Crabgame.Audio;
using UnityEngine;

namespace Crabgame.Entity
{
    public class SuicideBomber : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health health;

        [Header("Config")]
        [SerializeField] private int damageAmount;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Player"))
                return;

            var golem = other.collider.GetComponent<Health>();
            golem.TakeDamage(damageAmount);

            AudioPlayer.Instance.crabEnemyExplode.PlayOneShot();
            health.Die();
        }
    }
}
