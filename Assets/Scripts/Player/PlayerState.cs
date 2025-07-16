using System;
using UnityEngine;

namespace Shovel.Player
{
    [Serializable]
    public class PlayerState
    {
        [Min(1)] public int minionAmount;
        public int scrapAmount;
    }
}
