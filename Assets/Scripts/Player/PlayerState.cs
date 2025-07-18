using System;
using Crabgame.Managers;
using Unity.Properties;
using UnityEngine;

namespace Crabgame.Player
{
    [Serializable]
    public class PlayerState
    {
        public event Action OnBoughtArm;
        public event Action OnBoughtLeg;

        public          int scrapAmount;
        [Min(1)] public int minionAmount;

        [field: SerializeField] public bool GolemHasArm { get; private set; }
        [field: SerializeField] public bool GolemHasLeg { get; private set; }

        [CreateProperty] public bool CanBuyArm => !GolemHasArm && scrapAmount >= GameManager.Config.UpgradeArmCost;
        [CreateProperty] public bool CanBuyLeg => !GolemHasLeg && scrapAmount >= GameManager.Config.UpgradeLegCost;

#region Minion Upgrades
        [CreateProperty] public bool CanBuyMinions =>
            minionUpgradeIndex < MinionUpgrades.Length
            && scrapAmount >= MinionUpgrades[minionUpgradeIndex].scrapCost;

        [CreateProperty] public int NextMinionUpgradeCost =>
            CanBuyMinions ? MinionUpgrades[minionUpgradeIndex].scrapCost : -1;

        [CreateProperty] public string BuyMinionsText =>
            minionUpgradeIndex < MinionUpgrades.Length ?
                $"+{MinionUpgrades[minionUpgradeIndex].totalAmount} Minions"
                : "Minions Full!";

        private MinionUpgrade[] MinionUpgrades => GameManager.Config.MinionUpgrades;
        private int             minionUpgradeIndex;

        public void BuyMinions()
        {
            if (!CanBuyMinions)
                return;

            minionAmount =  MinionUpgrades[minionUpgradeIndex].totalAmount;
            scrapAmount  -= NextMinionUpgradeCost;

            minionUpgradeIndex++;
        }
#endregion

        public void BuyArm()
        {
            if (!CanBuyArm)
                return;

            GolemHasArm =  true;
            scrapAmount -= GameManager.Config.UpgradeArmCost;
            OnBoughtArm?.Invoke();
        }

        public void BuyLeg()
        {
            if (!CanBuyLeg)
                return;

            GolemHasLeg =  true;
            scrapAmount -= GameManager.Config.UpgradeLegCost;
            OnBoughtLeg?.Invoke();
        }

        public void Reset()
        {
            scrapAmount = 0;

            minionAmount       = 1;
            minionUpgradeIndex = 0;

            GolemHasArm = false;
            GolemHasLeg = false;
        }
    }
}
