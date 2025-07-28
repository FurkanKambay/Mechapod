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
        public event Action OnBoughtMinions;
        public event Action OnReset;

        public          int scrapAmount;
        [Min(1)] public int minionAmount;

        [field: SerializeField] public bool GolemHasArm { get; private set; }
        [field: SerializeField] public bool GolemHasLeg { get; private set; }

        [CreateProperty] public bool CanBuyArm => !GolemHasArm && scrapAmount >= GameManager.Config.UpgradeArmCost;
        [CreateProperty] public bool CanBuyLeg => !GolemHasLeg && scrapAmount >= GameManager.Config.UpgradeLegCost;

        [CreateProperty] public string BuyArmText => !GolemHasArm ? "Arm Upgrade" : "Arm Upgraded!";

#region Minion Upgrades
        [CreateProperty] public bool CanBuyMinions =>
            minionUpgradeIndex < MinionUpgrades.Length
            && scrapAmount >= MinionUpgrades[minionUpgradeIndex].scrapCost;

        [CreateProperty] public int NextMinionUpgradeCost =>
            CanBuyMinions ? MinionUpgrades[minionUpgradeIndex].scrapCost : -1;

        [CreateProperty] public string BuyMinionsText
        {
            get
            {
                if (minionUpgradeIndex >= MinionUpgrades.Length)
                    return "Frames Full!";

                int delta = MinionUpgrades[minionUpgradeIndex].totalAmount - minionAmount;
                return $"+{delta} Frame" + (delta == 1 ? "" : "s");
            }
        }

        private MinionUpgrade[] MinionUpgrades => GameManager.Config.MinionUpgrades;
        private int             minionUpgradeIndex;

        public void BuyMinions()
        {
            if (!CanBuyMinions)
                return;

            minionAmount =  MinionUpgrades[minionUpgradeIndex].totalAmount;
            scrapAmount  -= NextMinionUpgradeCost;

            minionUpgradeIndex++;
            OnBoughtMinions?.Invoke();
        }
#endregion

#region Golem Arms
        public void BuyArm()
        {
            if (!CanBuyArm)
                return;

            scrapAmount -= GameManager.Config.UpgradeArmCost;
            GiveArm();
        }

        public void GiveArm()
        {
            GolemHasArm = true;
            OnBoughtArm?.Invoke();
        }
#endregion

#region Golem Legs
        public void BuyLeg()
        {
            if (!CanBuyLeg)
                return;

            scrapAmount -= GameManager.Config.UpgradeLegCost;
            GiveLeg();
        }

        public void GiveLeg()
        {
            GolemHasLeg = true;
            OnBoughtLeg?.Invoke();
        }
#endregion

        public void Reset()
        {
            scrapAmount        = 0;
            minionAmount       = 1;
            minionUpgradeIndex = 0;

            GolemHasArm = false;
            GolemHasLeg = false;

            OnReset?.Invoke();
        }
    }
}
