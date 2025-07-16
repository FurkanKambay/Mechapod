using Shovel.Entity;
using UnityEngine;

namespace Shovel
{
    public class EnemyManager : SpawnerManager
    {
        [Header("References")]
        [SerializeField] private Health golem;

        protected override float MoveSpeed             => GameManager.Config.EnemyMoveSpeed;
        protected override float RandomAttackOffsetMax => GameManager.Config.EnemyRandomAttackOffsetMax;

        // TODO: use the correct WAVE
        private static int EntityAmount => GameManager.Tonight.Waves[0].EnemyAmount;

        private Transform golemTransform;

        private void Awake()
        {
            golemTransform = golem.transform;
            RespawnAll(EntityAmount);
        }

        private void FixedUpdate()
        {
            MoveAll(golemTransform.position);
        }
    }
}
