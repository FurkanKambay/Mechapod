using System;
using UnityEngine;

namespace Shovel.Night
{
    [Serializable]
    public class NightInfo
    {
        [SerializeField] internal int scrapPileAmount;
        [SerializeField] internal EnemyWave[] waves;

        public EnemyWave[] Waves => waves;

        public int GetEnemyAmount(int waveIndex)
        {
            if (waveIndex < 0 || waveIndex >= waves.Length)
                return 0;

            return waves[waveIndex].EnemyAmount;
        }
    }
}
