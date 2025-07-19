using System.Collections;
using Crabgame.Audio;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Entity
{
    public class SuicideBomber : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Attacker bomber;
        [SerializeField] private GameObject       explosionAnim;
        [SerializeField] private CircleCollider2D radiusTrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!bomber || !bomber.Health || bomber.Health.IsDead)
                return;

            if (!other.CompareTag("Player"))
                return;

            bomber.Health.enabled = false;
            bomber.enabled        = false;

            var golem = other.GetComponent<Health>();
            StartCoroutine(BlowUpWithDelay(golem));
        }

        private IEnumerator BlowUpWithDelay(Health target)
        {
            float delayMeters = radiusTrigger.radius / 2f;
            float timeToReach = delayMeters / bomber.MoveSpeed;

            yield return new WaitForSeconds(timeToReach);

            int damage = bomber.Health.EntityType switch
            {
                EntityType.EnemyMiniBoss => GameManager.Config.MiniBossExplosionDamage,
                _                        => GameManager.Config.MinionExplosionDamage
            };

            target.TakeDamage(damage);

            explosionAnim.SetActive(true);
            AudioPlayer.Instance.crabEnemyExplode.PlayOneShot();
            bomber.Health.Die();
        }
    }
}
