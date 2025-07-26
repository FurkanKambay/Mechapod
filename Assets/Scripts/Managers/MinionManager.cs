using UnityEngine;

namespace Crabgame.Managers
{
    public class MinionManager : SpawnerManager
    {
        [Header("References")]
        [SerializeField] private PlayerInput input;

        protected override float MoveSpeed => GameManager.Config.PlayerMoveSpeed;

        private void Awake()
        {
            RespawnAll(GameManager.PlayerState.minionAmount);
        }

        private void FixedUpdate()
        {
            MoveAll(input.AimPosition);
        }
    }
}
