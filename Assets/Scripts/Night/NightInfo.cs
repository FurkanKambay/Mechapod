using System;
using UnityEngine;

namespace Shovel.Night
{
    [Serializable]
    public class NightInfo
    {
        [SerializeField] internal EnemyWave[] waves;

        public EnemyWave[] Waves => waves;
    }
}
