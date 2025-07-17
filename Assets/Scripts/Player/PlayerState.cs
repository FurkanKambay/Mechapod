using System;
using Crabgame.Managers;
using Unity.Properties;
using UnityEngine;

namespace Crabgame.Player
{
    [Serializable]
    public class PlayerState
    {
        public int scrapAmount;

        [Min(1)] public int minionAmount;

        public bool GolemHasArm { get; private set; }
        public bool GolemHasLeg { get; private set; }

        [CreateProperty] public bool CanBuyArm => !GolemHasArm && scrapAmount >= GameManager.Config.UpgradeArmCost;
        [CreateProperty] public bool CanBuyLeg => !GolemHasLeg && scrapAmount >= GameManager.Config.UpgradeLegCost;

        public void BuyArm()
        {
            if (!CanBuyArm)
                return;

            GolemHasArm =  true;
            scrapAmount -= GameManager.Config.UpgradeArmCost;
        }

        public void BuyLeg()
        {
            if (!CanBuyLeg)
                return;

            GolemHasLeg =  true;
            scrapAmount -= GameManager.Config.UpgradeLegCost;
        }
    }
}
