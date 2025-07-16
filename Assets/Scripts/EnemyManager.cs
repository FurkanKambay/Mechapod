using Crabgame.Entity;
using UnityEngine;

namespace Crabgame
{
    public class EnemyManager : SpawnerManager
    {
        [Header("References")]
        [SerializeField] private Health golem;

        protected override float MoveSpeed             => GameManager.Config.EnemyMoveSpeed;
        protected override float RandomAttackOffsetMax => GameManager.Config.EnemyRandomAttackOffsetMax;

        private Transform golemTransform;

        private void Awake()
        {
            golemTransform = golem.transform;
            RespawnAll(GameManager.Tonight.Waves[0].EnemyAmount);
        }

        private void FixedUpdate()
        {
            if (golemTransform)
                MoveAll(golemTransform.position);
        }
    }
}
