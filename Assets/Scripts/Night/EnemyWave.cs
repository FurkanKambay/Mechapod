using System;
using UnityEngine;

namespace Crabgame.Night
{
    [Serializable]
    public class EnemyWave
    {
        [SerializeField, Range(1, 15)] private int enemyAmount = 1;
        [SerializeField, Range(0, 15)] private int miniBossAmount;

        public int EnemyAmount    => enemyAmount;
        public int MiniBossAmount => miniBossAmount;
    }
}
