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

            Clear();

            // HACK: replace with Player State > Minion Amount
            Spawn(2);
        }

        private void FixedUpdate()
        {
            MoveAll(input.AimPosition);
        }
    }
}
