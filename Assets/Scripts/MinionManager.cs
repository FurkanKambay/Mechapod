using UnityEngine;

namespace Shovel
{
    public class MinionManager : SpawnerManager
    {
        public static MinionManager Instance { get; private set; }

        [Header("References - Scene")]
        [SerializeField] private PlayerInput input;

        [Header("Config - Minions")]
        [SerializeField] private float moveSpeed;

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            Vector2 targetPoint = input.AimPosition;
            float   deltaTime   = Time.deltaTime;

            foreach (Rigidbody2D minion in entities)
            {
                if (!minion)
                    continue;

                Vector2 newPosition = Vector2.MoveTowards(minion.position, targetPoint, moveSpeed * deltaTime);
                minion.MovePosition(newPosition);
            }
        }
    }
}
