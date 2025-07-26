using Crabgame.Entity;
using UnityEngine;

namespace Crabgame.Managers
{
    public class EnemyManager : SpawnerManager
    {
        [Header("References")]
        [SerializeField] private Health golem;
        [SerializeField] private Attacker miniBossPrefab;

        protected override float MoveSpeed => GameManager.Config.EnemyMoveSpeed;

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

        public void SpawnMiniBosses(int amount) =>
            Spawn(miniBossPrefab, amount);
    }
}
