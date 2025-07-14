using UnityEngine;

namespace Shovel
{
    public class MinionManager : SpawnerManager
    {
        public static MinionManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerInput input;

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            MoveAll(input.AimPosition);
        }
    }
}
