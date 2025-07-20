using System.Collections;
using Crabgame.Entity;
using Crabgame.Managers;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Crabgame.Audio
{
    public enum DamageSource_FMOD
    {
        PlayerMinion,
        EnemyMinion,
        GolemLaser
    }

    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter musicEmitter;

        [Header("Entity")]
        public FMODEvent entityGetHurt;
        public FMODEvent entityDeath;

        [Header("Golem Skills")]
        public FMODEvent golemLaser;
        public FMODEvent golemLegStomp;

        [Header("Crabs")]
        public FMODEvent crabAttack;
        public FMODEvent crabAttackMiss;
        public FMODEvent crabEnemyExplode;
        public FMODEvent crabWalking;

        [Header("UI")]
        public FMODEvent uiGameOver;
        public FMODEvent uiGolemNewLimb;
        public FMODEvent uiNightSuccess;

        // Parameters
        private PARAMETER_DESCRIPTION paramEntityType;

        // private EventInstance eventInstance;

        private void OnEnable()
        {
            GameManager.Instance.OnPhaseChange += Game_PhaseChanged;
            GameManager.Golem.OnArmBeamStarted += Golem_BeamStarted;
            GameManager.Golem.OnArmBeamStopped += Golem_BeamStopped;

            GameManager.PlayerState.OnBoughtMinions += Golem_LimbUpgraded; // reuse
            GameManager.PlayerState.OnBoughtArm     += Golem_LimbUpgraded;
            GameManager.PlayerState.OnBoughtLeg     += Golem_LimbUpgraded;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPhaseChange -= Game_PhaseChanged;
            GameManager.Golem.OnArmBeamStarted -= Golem_BeamStarted;
            GameManager.Golem.OnArmBeamStopped -= Golem_BeamStopped;

            GameManager.PlayerState.OnBoughtMinions -= Golem_LimbUpgraded;
            GameManager.PlayerState.OnBoughtArm     -= Golem_LimbUpgraded;
            GameManager.PlayerState.OnBoughtLeg     -= Golem_LimbUpgraded;
        }

        private IEnumerator Start()
        {
            while (!RuntimeManager.HaveAllBanksLoaded)
                yield return null;

            entityGetHurt.Init();
            entityDeath.Init();

            golemLaser.Init();
            golemLegStomp.Init();

            crabAttack.Init();
            crabAttackMiss.Init();
            crabEnemyExplode.Init();
            crabWalking.Init();

            uiGameOver.Init();
            uiGolemNewLimb.Init();
            uiNightSuccess.Init();

            // - FMOD Parameters
            entityGetHurt.Description.getParameterDescriptionByName("Entity Type", out paramEntityType);
        }

        private EventInstance golemBeamInstance;

        private void Golem_BeamStarted() =>
            golemLaser.PlayOneShot();

        private void Golem_BeamStopped() =>
            golemLaser.Instance.stop(STOP_MODE.ALLOWFADEOUT);

        private void Golem_LimbUpgraded() =>
            uiGolemNewLimb.PlayOneShot();

        public void PlayEntityGetHurt(EntityType entityType)
        {
            entityGetHurt.Description.createInstance(out EventInstance instance);
            instance.setParameterByID(paramEntityType.id, entityType.GetHashCode());
            instance.start();
            instance.release();
        }

        public void PlayEntityDeath(EntityType entityType)
        {
            entityDeath.Description.createInstance(out EventInstance instance);
            instance.setParameterByID(paramEntityType.id, entityType.GetHashCode());
            instance.start();
            instance.release();
        }

        private void SetMusicPhase(bool isNight) => musicEmitter.SetParameter("Game Phase", isNight.GetHashCode());
        private void Game_PhaseChanged()         => SetMusicPhase(GameManager.Instance.IsNight);

#region Singleton
        public static AudioPlayer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
#endregion
    }
}
