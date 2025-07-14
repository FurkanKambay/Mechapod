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
            MoveAll(input.AimPosition, moveSpeed);
        }
    }
}
