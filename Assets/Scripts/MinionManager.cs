using UnityEngine;

namespace Shovel
{
    public class MinionManager : SpawnerManager
    {
        public static MinionManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerInput input;

        protected override int   EntityAmount          => GameManager.PlayerState.minionAmount;
        protected override float MoveSpeed             => GameManager.Config.PlayerMoveSpeed;
        protected override float RandomAttackOffsetMax => GameManager.Config.PlayerRandomAttackOffsetMax;

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
