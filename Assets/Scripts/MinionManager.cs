using System.Collections.Generic;
using UnityEngine;

namespace Shovel
{
    public class MinionManager : MonoBehaviour
    {
        [Header("References - Scene")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private List<Rigidbody2D> minions;

        [Header("References - Assets")]
        [SerializeField] private Rigidbody2D minionPrefab;

        [Header("Config - Minions")]
        [SerializeField] private float moveSpeed;

        [Header("Config - Spawning")]
        [SerializeField] private Vector3[] spawnPoints;

        public Vector3[] SpawnPoints => spawnPoints;

        private void FixedUpdate()
        {
            Vector2 targetPoint = input.AimPosition;
            float   deltaTime   = Time.deltaTime;

            foreach (Rigidbody2D minion in minions)
            {
                if (!minion)
                    continue;

                Vector2 newPosition = Vector2.MoveTowards(minion.position, targetPoint, moveSpeed * deltaTime);
                minion.MovePosition(newPosition);
            }
        }

        public void AddMinions(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int spawnedIndex = minions.Count + i;

                if (spawnedIndex >= spawnPoints.Length)
                {
                    Debug.Log("Exceeded max minions");
                    return;
                }

                var minion = Instantiate(minionPrefab, spawnPoints[spawnedIndex], Quaternion.identity, transform);
                minion.name = $"Minion {spawnedIndex + 1}";
                minions.Add(minion);
            }
        }

        public void ResetPositions()
        {
            for (var i = 0; i < minions.Count; i++)
            {
                Rigidbody2D minion = minions[i];

                if (!minion)
                    continue;

                minion.transform.position = spawnPoints[i];
            }
        }
    }
}
