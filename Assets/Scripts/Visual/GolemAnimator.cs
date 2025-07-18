using System;
using System.Collections;
using Crabgame.Audio;
using Crabgame.Managers;
using Crabgame.Player;
using UnityEngine;

namespace Crabgame.Visual
{
    public class GolemAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Golem golem;
        [SerializeField] private Animator       animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("References - Beam")]
        [SerializeField] private SpriteRenderer beamSource;
        [SerializeField] private SpriteRenderer beamSprite;

        [Header("References - Death")]
        [SerializeField] private GameObject[] explosions;

        [Header("Config")]
        [SerializeField, Min(0)] private float hurtDuration = 0.5f;
        [SerializeField, Min(0)] private float explodePadding = 0.2f;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private static readonly int AnimAttachArm = Animator.StringToHash("attach arm");
        private static readonly int AnimArmBlast  = Animator.StringToHash("arm blast");

#region Unity Lifetime
        private void Awake() =>
            propertyBlock = new MaterialPropertyBlock();

        private void OnEnable()
        {
            beamSprite.size = new Vector2(GameManager.Config.BeamLength, beamSprite.size.y);
            // beamSprite.transform.localScale = new Vector3(GameManager.Config.BeamLength, 1, 1);

            golem.OnGolemArm += Golem_ArmSkillUsed;

            golem.Health.OnHurt  += Health_Hurt;
            golem.Health.OnDeath += Health_Death;

            GameManager.PlayerState.OnBoughtArm += Golem_ArmAttached;
        }

        private void OnDisable()
        {
            golem.OnGolemArm -= Golem_ArmSkillUsed;

            golem.Health.OnHurt  -= Health_Hurt;
            golem.Health.OnDeath += Health_Death;

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

        private void Golem_ArmSkillUsed()
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

            beamSprite.enabled = true;
            golem.StartBeam();
            yield return new WaitForSeconds(GameManager.Config.BeamDuration);

            beamSprite.enabled = false;
            golem.StopBeam();

            beamSource.gameObject.SetActive(false);
            animator.SetBool(AnimArmBlast, false);
        }
    }
}
