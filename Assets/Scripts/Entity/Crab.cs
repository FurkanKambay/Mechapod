using System.Collections;
using Crabgame.Audio;
using Crabgame.Managers;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Crabgame.Entity
{
    public class Crab : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;

        private EventInstance footstepInstance;

        private IEnumerator Start()
        {
            while (!RuntimeManager.HaveAllBanksLoaded)
                yield return null;

            AudioPlayer.Instance.crabWalking.Description.createInstance(out footstepInstance);

            footstepInstance.start();
            footstepInstance.setVolume(0);
        }

        private void OnDestroy()
        {
            footstepInstance.stop(STOP_MODE.ALLOWFADEOUT);
            footstepInstance.release();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsNight)
                return;

            footstepInstance.setVolume(body.linearVelocity.magnitude > 0.1f ? 1f : 0f);
        }
    }
}
