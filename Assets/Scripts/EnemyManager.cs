using UnityEngine;

namespace Shovel
{
    public class EnemyManager : SpawnerManager
    {
        public static EnemyManager Instance { get; private set; }

        [Header("References - Scene")]
        [SerializeField] private Health golem;

        [Header("Config - Enemies")]
        [SerializeField] private float moveSpeed;

        private Transform golemTransform;

        private void Awake()
        {
            Instance       = this;
            golemTransform = golem.transform;
        }

        private void FixedUpdate()
        {
            MoveAll(golemTransform.position, moveSpeed);
        }
    }
}
