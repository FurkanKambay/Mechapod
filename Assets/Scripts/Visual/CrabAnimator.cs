using System.Collections;
using Crabgame.Audio;
using Crabgame.Entity;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crabgame.Visual
{
    public class CrabAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Attacker       attacker;
        [SerializeField] private Animator       animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private AnimationClip[] walkClips;
        [SerializeField] private AnimationClip[] attackClips;
        [SerializeField] private AnimationClip[] deathClips;

        [Header("Config")]
        [SerializeField, Min(0)] private float hurtDuration = 0.5f;
        [SerializeField, Min(0)] private float deathDestroyDelay = 2f;

        [Header("State")]
        [SerializeField] private AnimatorOverrideController animController;

        private MaterialPropertyBlock propertyBlock;

        private static readonly int ShaderHurt = Shader.PropertyToID("_Hurt");

        private static readonly int AnimAttack = Animator.StringToHash("attack");
        private static readonly int AnimDie    = Animator.StringToHash("die");

        private void Awake()
        {
            animController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            Assert.IsNotNull(animController);
            animator.runtimeAnimatorController = animController;

            propertyBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            attacker.OnAttackPerformed += Attack_Performed;
            attacker.OnAttackProcced   += Attack_Procced;
            attacker.Health.OnHurt     += Health_Hurt;
            attacker.Health.OnDeath    += Health_Death;
        }

        private void OnDisable()
        {
            attacker.OnAttackPerformed -= Attack_Performed;
            attacker.OnAttackProcced   -= Attack_Procced;
            attacker.Health.OnHurt     -= Health_Hurt;
            attacker.Health.OnDeath    -= Health_Death;
        }

        private void Update()
        {
            int clipIndex = (int)attacker.AimDirection - 1;
            animController[walkClips[0]]  = walkClips[clipIndex];
            animController[deathClips[0]] = deathClips[clipIndex];

            // can't turn: original animation keeps playing normally
            // else: swap to other animation (same playback position)
            if (!attacker.TurningBlocked)
                animController[attackClips[0]] = attackClips[clipIndex];
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
            animator.SetTrigger(AnimDie);
            Destroy(attacker.gameObject, deathDestroyDelay);
        }

        private void Attack_Performed() =>
            animator.SetTrigger(AnimAttack);

        private static void Attack_Procced(bool hitTarget)
        {
            if (hitTarget)
                AudioPlayer.Instance.crabAttack.PlayOneShot();
            else
                AudioPlayer.Instance.crabAttackMiss.PlayOneShot();
        }

        private void Anim_AttackProc() => attacker.ProcAttack();
        private void Anim_AttackDone() => attacker.isRecovering = false;
    }
}
