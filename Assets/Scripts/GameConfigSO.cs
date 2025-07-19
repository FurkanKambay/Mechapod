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
        [SerializeField] private float playerMoveSpeed;
        [SerializeField] private float enemyMoveSpeed;

        public float PlayerMoveSpeed => playerMoveSpeed;
        public float EnemyMoveSpeed  => enemyMoveSpeed;

        // Crab Attacks
        [Header("Minions & Boss - Attack")]
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

        // Attack Offset
        [Header("Attack Offset (Random Max)")]
        [SerializeField] private float playerRandomAttackOffsetMax;
        [SerializeField] private float enemyRandomAttackOffsetMax;

        public float PlayerRandomAttackOffsetMax => playerRandomAttackOffsetMax;
        public float EnemyRandomAttackOffsetMax  => enemyRandomAttackOffsetMax;

        // Explosion
        [Header("Minions & Boss - Explosion")]
        [SerializeField] private int minionExplosionDamage;
        [SerializeField] private int miniBossExplosionDamage;

        public int MinionExplosionDamage   => minionExplosionDamage;
        public int MiniBossExplosionDamage => miniBossExplosionDamage;
#endregion

#region Economy
        [Header("Economy")]
        [SerializeField] private int upgradeArmCost;
        [SerializeField] private int             upgradeLegCost;
        [SerializeField] private MinionUpgrade[] minionUpgrades;

        public int             UpgradeArmCost => upgradeArmCost;
        public int             UpgradeLegCost => upgradeLegCost;
        public MinionUpgrade[] MinionUpgrades => minionUpgrades;
#endregion
    }
}
