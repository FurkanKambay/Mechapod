using System;
using UnityEngine;

namespace Crabgame.Night
{
    [Serializable]
    public class EnemyWave
    {
        [Min(1)]
        [SerializeField] private int enemyAmount = 1;

        public int EnemyAmount => enemyAmount;
    }
}
