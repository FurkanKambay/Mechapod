using System.Collections.Generic;
using Shovel.Entity;
using UnityEditor;
using UnityEngine;

namespace Shovel
{
    public abstract class SpawnerManager : MonoBehaviour
    {
        [Header("Base - References")]
        [SerializeField] protected Attacker entityPrefab;

        [Header("Base - Config")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] private   float     randomAttackOffsetMax;
        [SerializeField] protected Vector3[] spawnPoints;

        [Header("Base - State")]
        [SerializeField] protected string dummy;
        [SerializeField] protected List<Attacker> entities;

        public Vector3[] SpawnPoints => spawnPoints;

        protected abstract int EntityAmount { get; }

        protected void MoveAll(Vector2 targetPoint)
        {
            foreach (Attacker entity in entities)
            {
                if (!entity)
                    continue;

                Vector2 entityPosition = entity.transform.position;
                Vector2 direction      = Vector2.ClampMagnitude(targetPoint - entityPosition, 1f);

                entity.Body.linearVelocity = direction * moveSpeed;
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

                Attacker attacker = Instantiate(
                    entityPrefab,
                    spawnPoints[spawnedIndex],
                    Quaternion.identity,
                    transform
                );

                attacker.name = $"{entityName} {spawnedIndex + 1}";

                // TODO: try manually setting the offset from Spawner
                attacker.attackOffset = Random.value * randomAttackOffsetMax;

                entities.Add(attacker);
            }
        }

        public void RespawnAll()
        {
            Clear();
            Spawn(EntityAmount);
        }

        public void ResetPositions()
        {
            for (var i = 0; i < entities.Count; i++)
            {
                Attacker entity = entities[i];

                if (!entity)
                    continue;

                entity.transform.position = spawnPoints[i];
            }
        }

        public void Clear()
        {
            foreach (Attacker entity in entities)
            {
                if (entity)
                    Destroy(entity.gameObject);
            }

            entities.Clear();
        }

        private void OnDrawGizmos()
        {
            foreach (Attacker entity in entities)
            {
                if (!entity)
                    return;

                Handles.color = Color.green;
                Handles.DrawWireDisc(entity.transform.position, Vector3.forward, entity.AttackRadius);
            }
        }
    }
}
