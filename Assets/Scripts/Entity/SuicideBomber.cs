using System;
using Crabgame.Audio;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Entity
{
    public class SuicideBomber : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private GameObject explosionAnim;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!health || health.IsDead)
                return;

            if (!other.CompareTag("Player"))
                return;

            var golem = other.GetComponent<Health>();
            golem.TakeDamage(health.EntityType switch
            {
                EntityType.EnemyMiniBoss => GameManager.Config.MiniBossExplosionDamage,
                _                        => GameManager.Config.MinionExplosionDamage
            });

            explosionAnim.SetActive(true);
            AudioPlayer.Instance.crabEnemyExplode.PlayOneShot();

            health.Die();
        }
    }
}
