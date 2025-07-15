using Shovel.Entity;
using UnityEngine;

namespace Shovel
{
    public class EnemyManager : SpawnerManager
    {
        public static EnemyManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private Health golem;

        // TODO: use the correct WAVE
        protected override int EntityAmount =>
            GameManager.Tonight.Waves[0].EnemyAmount;

        private Transform golemTransform;

        private void Awake()
        {
            Instance       = this;
            golemTransform = golem.transform;

            RespawnAll();
        }

        private void FixedUpdate()
        {
            MoveAll(golemTransform.position);
        }
    }
}
