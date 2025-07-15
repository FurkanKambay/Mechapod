using UnityEngine;

namespace Shovel
{
    public class MinionManager : SpawnerManager
    {
        public static MinionManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerInput input;

        protected override int EntityAmount => GameManager.PlayerState.minionAmount;

        private void Awake()
        {
            Instance = this;
            RespawnAll();
        }

        private void FixedUpdate()
        {
            MoveAll(input.AimPosition);
        }
    }
}
