using UnityEngine;

namespace Shovel
{
    public class EnemyManager : SpawnerManager
    {
        public static EnemyManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private Health golem;

        private Transform golemTransform;

        private void Awake()
        {
            Instance       = this;
            golemTransform = golem.transform;
        }

        private void FixedUpdate()
        {
            MoveAll(golemTransform.position);
        }
    }
}
