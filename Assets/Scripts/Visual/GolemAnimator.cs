using System.Collections;
using Crabgame.Entity;
using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Visual
{
    public class GolemAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private Animator       animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject[]   explosions;

        [Header("Config")]
        [SerializeField, Min(0)] private float hurtDuration = 0.5f;
        [SerializeField, Min(0)] private float explodePadding = 0.2f;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private static readonly int AnimAttachArm = Animator.StringToHash("attach arm");
        private static readonly int AnimArmBlast  = Animator.StringToHash("arm blast");

        private void Awake() =>
            propertyBlock = new MaterialPropertyBlock();

        private void OnEnable()
        {
            health.OnHurt  += Health_Hurt;
            health.OnDeath += Health_Death;

            GameManager.PlayerState.OnBoughtArm += Golem_ArmAttached;
            // TODO: Golem Arm Skill event
        }

        private void OnDisable()
        {
            health.OnHurt  -= Health_Hurt;
            health.OnDeath += Health_Death;

            GameManager.PlayerState.OnBoughtArm -= Golem_ArmAttached;
        }

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
            StartCoroutine(ExplodeAndDestroy());
        }

        private IEnumerator ExplodeAndDestroy()
        {
            foreach (GameObject explode in explosions)
            {
                explode.SetActive(true);
                yield return new WaitForSeconds(explodePadding);
            }

            yield return new WaitForSeconds(explodePadding);
            Destroy(gameObject);
        }

        private void Golem_ArmAttached() =>
            animator.SetTrigger(AnimAttachArm);

        private void Golem_ArmBlasted() =>
            animator.SetTrigger(AnimArmBlast);
    }
}
