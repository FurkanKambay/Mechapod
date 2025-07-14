using System.Collections.Generic;
using UnityEngine;

namespace Shovel
{
    public abstract class SpawnerManager : MonoBehaviour
    {
        [Header("Base - References")]
        [SerializeField] protected Rigidbody2D entityPrefab;

        [Header("Base - Config")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected Vector3[] spawnPoints;

        [Header("Base - State")]
        [SerializeField] protected string dummy;
        [SerializeField] protected List<Rigidbody2D> entities;

        public Vector3[] SpawnPoints => spawnPoints;

        protected void MoveAll(Vector2 targetPoint)
        {
            foreach (Rigidbody2D entity in entities)
            {
                if (!entity)
                    continue;

                Vector2 direction = Vector2.ClampMagnitude(targetPoint - entity.position, 1f);
                entity.linearVelocity = direction * moveSpeed;
            }
        }

        public void Spawn(int count)
        {
            string entityName = entityPrefab.name;

            for (int i = 0; i < count; i++)
            {
                int spawnedIndex = entities.Count + i;

                if (spawnedIndex >= spawnPoints.Length)
                {
                    Debug.Log($"Exceeded max {entityName}s");
                    return;
                }

                var entity = Instantiate(entityPrefab, spawnPoints[spawnedIndex], Quaternion.identity, transform);
                entity.name = $"{entityName} {spawnedIndex + 1}";
                entities.Add(entity);
            }
        }

        public void ResetPositions()
        {
            for (var i = 0; i < entities.Count; i++)
            {
                Rigidbody2D entity = entities[i];

                if (!entity)
                    continue;

                entity.transform.position = spawnPoints[i];
            }
        }

        public void Clear()
        {
            foreach (Rigidbody2D enemy in entities)
            {
                if (enemy)
                    Destroy(enemy.gameObject);
            }

            entities.Clear();
        }
    }
}
