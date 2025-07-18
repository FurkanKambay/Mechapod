using System;
using UnityEngine;

namespace Crabgame
{
    [Serializable]
    public struct MinionUpgrade
    {
        public int totalAmount;
        public int scrapCost;
    }

    [CreateAssetMenu]
    public sealed class GameConfigSO : ScriptableObject
    {
        [Header("Game")]
        [Tooltip("How long to wait after a successful night")]
        [SerializeField] private float nightWaitTime = 1f;

        [Tooltip("How long to wait after a game over to restart")]
        [SerializeField] private float gameRestartTime = 4f;

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

        public float NightWaitTime   => nightWaitTime;
        public float GameRestartTime => gameRestartTime;

        public float PlayerMoveSpeed => playerMoveSpeed;
        public float EnemyMoveSpeed  => enemyMoveSpeed;

        public bool MoveWhileAttacking  => moveWhileAttacking;
        public bool MoveWhileRecovering => moveWhileRecovering;
        public bool TurnWhileAttacking  => turnWhileAttacking;
        public bool TurnWhileRecovering => turnWhileRecovering;

        public float PlayerRandomAttackOffsetMax => playerRandomAttackOffsetMax;
        public float EnemyRandomAttackOffsetMax  => enemyRandomAttackOffsetMax;

        [Header("Economy")]
        [SerializeField] private int upgradeArmCost;
        [SerializeField] private int             upgradeLegCost;
        [SerializeField] private MinionUpgrade[] minionUpgrades;

        public int             UpgradeArmCost => upgradeArmCost;
        public int             UpgradeLegCost => upgradeLegCost;
        public MinionUpgrade[] MinionUpgrades => minionUpgrades;
    }
}
