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
        [Tooltip("Can crabs move while attacking?")]
        [SerializeField] private bool moveWhileAttacking;

        [Tooltip("Can crabs move while recovering after an attack?")]
        [SerializeField] private bool moveWhileRecovering;

        [Tooltip("Can crabs turn while attacking?")]
        [SerializeField] private bool turnWhileAttacking;

        [Tooltip("Can crabs turn while recovering after an attack?")]
        [SerializeField] private bool turnWhileRecovering;

        [Header("Attack Offset (Random Max)")]
        [SerializeField] private float playerRandomAttackOffsetMax;
        [SerializeField] private float enemyRandomAttackOffsetMax;

        public float PlayerMoveSpeed => playerMoveSpeed;
        public float EnemyMoveSpeed  => enemyMoveSpeed;

        public bool MoveWhileAttacking  => moveWhileAttacking;
        public bool MoveWhileRecovering => moveWhileRecovering;
        public bool TurnWhileAttacking  => turnWhileAttacking;
        public bool TurnWhileRecovering => turnWhileRecovering;

        public float PlayerRandomAttackOffsetMax => playerRandomAttackOffsetMax;
        public float EnemyRandomAttackOffsetMax  => enemyRandomAttackOffsetMax;
    }
}
