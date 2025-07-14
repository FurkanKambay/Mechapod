using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Shovel.Audio
{
    [Serializable]
    public struct FMODEvent
    {
        [SerializeField] private EventReference reference;

        public EventReference   Reference   => reference;
        public EventDescription Description => description;
        public EventInstance    Instance    => instance;

        private EventDescription description;
        private EventInstance    instance;

        public void Init() =>
            description = RuntimeManager.GetEventDescription(reference);

        public void PlayOneShot()
        {
            if (!Description.isValid())
            {
                Debug.LogWarning($"Invalid FMOD event description: {reference.Path}", AudioPlayer.Instance);
                return;
            }

            Description.createInstance(out instance);
            Instance.start();
            Instance.release();
        }
    }
}
