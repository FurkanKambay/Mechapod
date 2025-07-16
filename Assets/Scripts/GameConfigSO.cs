using UnityEngine;

namespace Shovel
{
    [CreateAssetMenu]
    public sealed class GameConfigSO : ScriptableObject
    {
        [Header("Move Speed")]
        [SerializeField] private float playerMoveSpeed;
        [SerializeField] private float enemyMoveSpeed;

        [Header("Attack")]
        [Tooltip("Can we turn in another direction before the attack proc's?")]
        [SerializeField] private bool turnWhileAttacking;

        [Header("Attack Offset (Random Max)")]
        [SerializeField] private float playerRandomAttackOffsetMax;
        [SerializeField] private float enemyRandomAttackOffsetMax;

        public float PlayerMoveSpeed => playerMoveSpeed;
        public float EnemyMoveSpeed  => enemyMoveSpeed;

        public bool TurnWhileAttacking => turnWhileAttacking;

        public float PlayerRandomAttackOffsetMax => playerRandomAttackOffsetMax;
        public float EnemyRandomAttackOffsetMax  => enemyRandomAttackOffsetMax;
    }
}
