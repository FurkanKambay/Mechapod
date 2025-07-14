using UnityEngine;

namespace Shovel
{
    public class EnemyManager : SpawnerManager
    {
        public static EnemyManager Instance { get; private set; }

        [Header("References - Scene")]
        [SerializeField] private Health golem;

        [Header("Config - Enemies")]
        [SerializeField] private float moveSpeed;

        private Transform golemTransform;

        private void Awake()
        {
            Instance       = this;
            golemTransform = golem.transform;
        }

        private void FixedUpdate()
        {
            Vector2 targetPoint = golemTransform.position;
            float   deltaTime   = Time.deltaTime;

            foreach (Rigidbody2D enemy in entities)
            {
                if (!enemy)
                    continue;

                Vector2 newPosition = Vector2.MoveTowards(enemy.position, targetPoint, moveSpeed * deltaTime);
                enemy.MovePosition(newPosition);
            }
        }
    }
}
