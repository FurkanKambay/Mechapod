using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Crabgame.Audio
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
            bool success = CreateInstance();

            if (!success)
                return;

            instance.release();
        }

        public bool CreateInstance()
        {
            if (!Description.isValid())
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Invalid FMOD event description: {reference.Path}", AudioPlayer.Instance);
#endif
                return false;
            }

            Description.createInstance(out instance);
            instance.start();
            return true;
        }
    }
}
