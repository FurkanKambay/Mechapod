using UnityEngine;

namespace Shovel
{
    public class MinionManager : SpawnerManager
    {
        [Header("References")]
        [SerializeField] private PlayerInput input;

        protected override float MoveSpeed             => GameManager.Config.PlayerMoveSpeed;
        protected override float RandomAttackOffsetMax => GameManager.Config.PlayerRandomAttackOffsetMax;

        private static int EntityAmount => GameManager.PlayerState.minionAmount;

        private void Awake()
        {
            RespawnAll(EntityAmount);
        }

        private void FixedUpdate()
        {
            MoveAll(input.AimPosition);
        }
    }
}
