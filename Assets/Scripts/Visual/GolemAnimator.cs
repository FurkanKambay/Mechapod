using System.Collections;
using Crabgame.Audio;
using Crabgame.Managers;
using Crabgame.Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Crabgame.Visual
{
    public class GolemAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Golem golem;
        [SerializeField] private Animator       animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Light2D        globalLight;

        [Header("References - Beam")]
        [SerializeField] private SpriteRenderer beamSource;
        [SerializeField] private SpriteRenderer beamSprite;
        [SerializeField] private Light2D        beamLight;

        [Header("References - Death")]
        [SerializeField] private GameObject[] explosions;

        [Header("Config")]
        [SerializeField, Min(0)] private float hurtDuration = 0.5f;
        [SerializeField, Min(0)] private float explodePadding = 0.2f;

        [Header("Config - Lighting")]
        [SerializeField, Min(0)] private float lightIntensity = 1f;
        [SerializeField, Min(0)] private float lightIntensityBeaming = 0.5f;

        private static GameConfigSO Config => GameManager.Config;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private static readonly int AnimAttachArm = Animator.StringToHash("attach arm");
        private static readonly int AnimArmBlast  = Animator.StringToHash("arm blast");

#region Unity Lifetime
        private void Awake() =>
            propertyBlock = new MaterialPropertyBlock();

        private void OnEnable()
        {
            beamSprite.size = new Vector2(Config.BeamLength, Config.BeamWidth);

            Transform lightTransform = beamLight.transform;
            lightTransform.localScale    = new Vector3(Config.BeamLength,     Config.BeamWidth, 1);
            lightTransform.localPosition = new Vector3(Config.BeamLength / 2, 0,                0);

            golem.OnArmBeamTriggered += Golem_ArmBeamTriggered;

            golem.Health.OnHurt  += Health_Hurt;
            golem.Health.OnDeath += Health_Death;

            GameManager.PlayerState.OnBoughtArm += Golem_ArmAttached;
        }

        private void OnDisable()
        {
            golem.OnArmBeamTriggered -= Golem_ArmBeamTriggered;

            golem.Health.OnHurt  -= Health_Hurt;
            golem.Health.OnDeath -= Health_Death;

            GameManager.PlayerState.OnBoughtArm -= Golem_ArmAttached;
        }

        private void Update()
        {
            beamSprite.enabled = golem.IsBeaming;

            if (golem.IsBeaming)
                beamSprite.transform.rotation = Quaternion.Euler(0, 0, golem.BeamAngle);
        }
#endregion

#region Health Events
        private void Health_Hurt()
        {
            propertyBlock.SetInt(ShaderHurt, 1);
            spriteRenderer.SetPropertyBlock(propertyBlock);

            StartCoroutine(RevertShader(hurtDuration));
        }

        private IEnumerator RevertShader(float delay)
        {
            yield return new WaitForSeconds(delay);
            propertyBlock.SetInt(ShaderHurt, 0);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void Health_Death()
        {
            propertyBlock.SetInt(ShaderHurt, 1);
            spriteRenderer.SetPropertyBlock(propertyBlock);
            // Health_Hurt();

            StartCoroutine(ExplodeAndDisable());
        }

        private IEnumerator ExplodeAndDisable()
        {
            foreach (GameObject explode in explosions)
            {
                yield return new WaitForSeconds(explodePadding);

                AudioPlayer.Instance.crabEnemyExplode.PlayOneShot();
                explode.SetActive(true);
            }

            yield return new WaitForSeconds(explodePadding);
            transform.parent.gameObject.SetActive(false);

            // reset explosions for revival
            foreach (GameObject explode in explosions)
                explode.SetActive(false);
        }
#endregion

        private void Golem_ArmAttached() =>
            animator.SetTrigger(AnimAttachArm);

        private void Golem_ArmBeamTriggered()
        {
            animator.SetBool(AnimArmBlast, true);
            // wait for animation event callback
        }

        private void Anim_RunBeam() =>
            StartCoroutine(StartBeam());

        private IEnumerator StartBeam()
        {
            beamSource.gameObject.SetActive(true);
            yield return new WaitForSeconds(GameManager.Config.BeamDelay);

            // sprite + light
            beamSprite.gameObject.SetActive(true);
            globalLight.intensity = lightIntensityBeaming;

            golem.StartBeam();
            yield return new WaitForSeconds(GameManager.Config.BeamDuration);

            // revert light
            globalLight.intensity = lightIntensity;

            // source + beam sprites, anim
            beamSprite.gameObject.SetActive(false);
            beamSource.gameObject.SetActive(false);
            animator.SetBool(AnimArmBlast, false);

            golem.StopBeam();
        }
    }
}
