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
#region Game
        [Header("Game")]
        [Tooltip("How long to wait after a successful night")]
        [SerializeField] private float nightWaitTime = 1f;

        [Tooltip("How long to wait after a game over to restart")]
        [SerializeField] private float gameRestartTime = 4f;

        public float NightWaitTime   => nightWaitTime;
        public float GameRestartTime => gameRestartTime;
#endregion

#region Golem
        [Header("Golem - Beam Time")]
        [SerializeField, Min(0)] private float beamDelay = 1f;
        public float BeamDelay => beamDelay;

        [SerializeField, Min(1)]
        private float beamDuration = 4f;
        public float BeamDuration => beamDuration;

        [Header("Golem - Beam Shape")]
        [SerializeField, Min(1)]
        private float beamLength = 5f;
        public float BeamLength => beamLength;

        [SerializeField, Min(0f)]
        private float beamWidth = 1f;
        public float BeamWidth => beamWidth;

        [Header("Golem - Beam Damage & Speed")]
        [SerializeField, Min(0f)]
        private int beamDamage = 10;
        public int BeamDamage => beamDamage;

        [SerializeField, Min(0f)]
        private float beamDamageRate = 1f;
        public float BeamDamageRate => beamDamageRate;

        public float BeamFollowSpeed => beamFollowSpeed;
        [SerializeField, Min(10f)]
        private float beamFollowSpeed = 200f;
#endregion

#region Minions
        [Header("Minions - Move Speed")]
        [SerializeField] private float playerMoveSpeed = 1.2f;
        [SerializeField] private float enemyMoveSpeed = 0.8f;
        [SerializeField] private float bossMoveSpeed  = 0.4f;

        public float PlayerMoveSpeed => playerMoveSpeed;
        public float EnemyMoveSpeed  => enemyMoveSpeed;
        public float BossMoveSpeed   => bossMoveSpeed;

        // Attacks
        [Header("Minions - Attack")]
        [SerializeField, Min(0f)] private int playerAttackDamage = 10;
        [SerializeField, Min(0f)] private float playerAttackRange = 1.2f;
        [SerializeField, Min(0f)] private float playerAttackRate  = 1.2f;

        [SerializeField, Min(0f)] private int   enemyAttackDamage = 10;
        [SerializeField, Min(0f)] private float enemyAttackRange  = 1f;
        [SerializeField, Min(0f)] private float enemyAttackRate   = 2f;

        [SerializeField, Min(0f)] private int   bossAttackDamage = 50;
        [SerializeField, Min(0f)] private float bossAttackRange  = 2f;
        [SerializeField, Min(0f)] private float bossAttackRate   = 3.5f;

        public int   PlayerAttackDamage => playerAttackDamage;
        public float PlayerAttackRange  => playerAttackRange;
        public float PlayerAttackRate   => playerAttackRate;

        public int   EnemyAttackDamage => enemyAttackDamage;
        public float EnemyAttackRange  => enemyAttackRange;
        public float EnemyAttackRate   => enemyAttackRate;

        public int   BossAttackDamage => bossAttackDamage;
        public float BossAttackRange  => bossAttackRange;
        public float BossAttackRate   => bossAttackRate;

        // Explosion
        [Header("Minions & Boss - Explosion")]
        [SerializeField] private int minionExplosionDamage;
        [SerializeField] private int miniBossExplosionDamage;

        public int MinionExplosionDamage   => minionExplosionDamage;
        public int MiniBossExplosionDamage => miniBossExplosionDamage;
#endregion

#region Attack Strategy
        // Crabs Attack Strategy
        [Header("Minions & Boss - Attack Strategy")]
        [Tooltip("Can crabs move while attacking?")]
        [SerializeField] private bool moveWhileAttacking;

        [Tooltip("Can crabs move while recovering after an attack?")]
        [SerializeField] private bool moveWhileRecovering;

        [Tooltip("Can crabs turn while attacking?")]
        [SerializeField] private bool turnWhileAttacking;

        [Tooltip("Can crabs turn while recovering after an attack?")]
        [SerializeField] private bool turnWhileRecovering;

        public bool MoveWhileAttacking  => moveWhileAttacking;
        public bool MoveWhileRecovering => moveWhileRecovering;
        public bool TurnWhileAttacking  => turnWhileAttacking;
        public bool TurnWhileRecovering => turnWhileRecovering;
#endregion

#region Economy
        [Header("Economy")]
        [SerializeField, Min(1)] private int scrapsPerPile = 10;
        [SerializeField, Min(1)] private int upgradeArmCost = 25;
        [SerializeField, Min(1)] private int upgradeLegCost = 100;

        [SerializeField] private MinionUpgrade[] minionUpgrades;

        public int             ScrapsPerPile  => scrapsPerPile;
        public int             UpgradeArmCost => upgradeArmCost;
        public int             UpgradeLegCost => upgradeLegCost;
        public MinionUpgrade[] MinionUpgrades => minionUpgrades;
#endregion
    }
}
