using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shovel
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("References - Assets")]
        [SerializeField] private Rigidbody2D enemyPrefab;

        [Header("References - Scene")]
        [SerializeField] private Health golem;
        [SerializeField] private List<Rigidbody2D> enemies;

        [Header("Config - Enemies")]
        [SerializeField] private float moveSpeed;

        [Header("Config - Spawning")]
        [SerializeField] private Vector3[] spawnPoints;

        private Transform golemTransform;

        private void Awake()
        {
            golemTransform = golem.transform;
        }

        private void Update()
        {
            Vector2 targetPoint = golemTransform.position;
            float   deltaTime   = Time.deltaTime;

            foreach (Rigidbody2D enemy in enemies)
            {
                if (!enemy)
                    continue;

                Vector2 newPosition = Vector2.MoveTowards(enemy.position, targetPoint, moveSpeed * deltaTime);
                enemy.MovePosition(newPosition);
            }
        }

        public void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int spawnedIndex = enemies.Count + i;

                if (spawnedIndex >= spawnPoints.Length)
                {
                    Debug.Log("Exceeded max enemies");
                    return;
                }

                var enemy = Instantiate(enemyPrefab, spawnPoints[spawnedIndex], Quaternion.identity, transform);
                enemy.name = $"Enemy {spawnedIndex + 1}";
                enemies.Add(enemy);
            }
        }
    }
}
