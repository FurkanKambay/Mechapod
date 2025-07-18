using System;
using UnityEngine;

namespace Crabgame.Night
{
    [CreateAssetMenu]
    public sealed class NightMapSO : ScriptableObject
    {
        [Header("Special")]
        [SerializeField, Min(1)] private int miniBossNight = 10;
        public int MiniBossNight => miniBossNight;

        [Header("Night Map")]
        [Min(1)]
        [SerializeField] private int nightCount = 10;

        [SerializeField] private NightInfo[] nights;

        public int NightCount => nightCount;

        public NightInfo[] Nights => nights;

        public NightInfo GetNight(int dayIndex)
        {
            if (nights.Length == 0)
                return null;

            if (dayIndex < 0 || dayIndex >= nights.Length)
                return nights[^1];

            return nights[dayIndex];
        }

        private void OnValidate()
        {
            miniBossNight = Math.Clamp(miniBossNight, 1, nightCount);

            Array.Resize(ref nights, nightCount);

            foreach (NightInfo nightInfo in nights)
            {
                if (nightInfo.Waves == null || nightInfo.Waves.Length == 0)
                    Array.Resize(ref nightInfo.waves, 1);
            }
        }
    }
}
