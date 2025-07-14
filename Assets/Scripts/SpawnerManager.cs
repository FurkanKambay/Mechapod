using System.Collections.Generic;
using UnityEngine;

namespace Shovel
{
    public abstract class SpawnerManager : MonoBehaviour
    {
        [Header("References - Scene")]
        [SerializeField] protected List<Rigidbody2D> entities;

        [Header("References - Assets")]
        [SerializeField] protected Rigidbody2D entityPrefab;

        [Header("Config - Spawning")]
        [SerializeField] protected Vector3[] spawnPoints;

        public Vector3[] SpawnPoints => spawnPoints;

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
    }
}
