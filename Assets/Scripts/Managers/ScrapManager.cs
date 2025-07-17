using System.Collections.Generic;
using Crabgame.Entity;
using UnityEngine;

namespace Crabgame.Managers
{
    public sealed class ScrapManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScrapPile prefab;
        [SerializeField] private PolygonCollider2D polygonCollider;

        [Header("State")]
        [SerializeField] private List<ScrapPile> piles;

        public void RespawnAll(int amount)
        {
            Clear();
            Spawn(amount);
        }

        [ContextMenu("Respawn 5")]
        private void Respawn5() => RespawnAll(5);

        private void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 point = GetRandomPoint();

                ScrapPile pile = Instantiate(prefab, point, Quaternion.identity, transform);
                pile.name = $"{prefab.name} {piles.Count + 1}";

                piles.Add(pile);
            }
        }

        private Vector2 GetRandomPoint()
        {
            Bounds  bounds = polygonCollider.bounds;
            Vector2 point;

            while (true)
            {
                point = new Vector2(
                    x: Random.Range(bounds.min.x, bounds.max.x),
                    y: Random.Range(bounds.min.y, bounds.max.y)
                );

                if (polygonCollider.OverlapPoint(point) && CanSpawnAt(point))
                    break;
            }

            return point;
        }

        private bool CanSpawnAt(Vector2 point)
        {
            foreach (ScrapPile pile in piles)
            {
                if (!pile)
                    continue;

                if (Vector2.SqrMagnitude((Vector2)pile.transform.position - point) <= 1)
                    return false;
            }

            return true;
        }

        private void Clear()
        {
            foreach (ScrapPile pile in piles)
            {
                if (pile)
                    Destroy(pile.gameObject);
            }

            piles.Clear();
        }
    }
}
