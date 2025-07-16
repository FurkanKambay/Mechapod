using System;
using UnityEngine;

namespace Crabgame.Player
{
    [Serializable]
    public class PlayerState
    {
        [Min(1)] public int minionAmount;
        public int scrapAmount;
    }
}
