using System.Collections;
using UnityEngine;
using FMODUnity;

namespace Crabgame.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter musicEmitter;

        public FMODEvent golemLaser;
        public FMODEvent golemLegStomp;
        public FMODEvent golemDie;
        public FMODEvent golemGetHurt;

        public FMODEvent crabAttack;
        public FMODEvent crabAttackMiss;
        public FMODEvent crabEnemyExplode;
        public FMODEvent crabGetHurt;
        public FMODEvent crabWalking;

        public FMODEvent uiGameOver;
        public FMODEvent uiGolemNewLimb;
        public FMODEvent uiNightSuccess;

        // private EventInstance eventInstance;

        private IEnumerator Start()
        {
            while (!RuntimeManager.HaveAllBanksLoaded)
                yield return null;

            golemLaser.Init();
            golemLegStomp.Init();
            golemDie.Init();
            golemGetHurt.Init();
            crabAttack.Init();
            crabAttackMiss.Init();
            crabEnemyExplode.Init();
            crabGetHurt.Init();
            crabWalking.Init();
            uiGameOver.Init();
            uiGolemNewLimb.Init();
            uiNightSuccess.Init();
        }

#region Singleton
        public static AudioPlayer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
#endregion
    }
}
