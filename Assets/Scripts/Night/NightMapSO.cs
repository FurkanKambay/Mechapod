using System;
using UnityEngine;

namespace Shovel.Night
{
    [CreateAssetMenu]
    public sealed class NightMapSO : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int nightCount = 10;

        [SerializeField] private NightInfo[] nights;

        public NightInfo[] Nights => nights;

        private void OnValidate()
        {
            Array.Resize(ref nights, nightCount);

            foreach (NightInfo nightInfo in nights)
            {
                if (nightInfo.Waves == null || nightInfo.Waves.Length == 0)
                    Array.Resize(ref nightInfo.waves, 1);
            }
        }
    }
}
