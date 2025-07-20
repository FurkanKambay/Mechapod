using System;
using UnityEngine;

namespace Crabgame.Night
{
    [Serializable]
    public class NightInfo
    {
        [Min(0)]
        [SerializeField] internal int scrapPileAmount;
        [SerializeField] internal EnemyWave[] waves;

        public EnemyWave[] Waves => waves;

        public int GetEnemyAmount(int waveIndex)
        {
            if (waveIndex < 0 || waveIndex >= waves.Length)
                return 0;

            return waves[waveIndex].EnemyAmount;
        }

        public int GetMiniBossAmount(int waveIndex)
        {
            if (waveIndex < 0 || waveIndex >= waves.Length)
                return 0;

            return waves[waveIndex].MiniBossAmount;
        }
    }
}
