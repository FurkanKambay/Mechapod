using System;
using UnityEngine;

namespace Crabgame.Night
{
    [Serializable]
    public class EnemyWave
    {
        [Min(1)]
        [SerializeField] private int enemyAmount = 1;

        [Min(0)]
        [SerializeField] private int scrapAmount;

        public int EnemyAmount => enemyAmount;
        public int ScrapAmount => scrapAmount;
    }
}
