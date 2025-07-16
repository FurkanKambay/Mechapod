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
        [SerializeField] private Attacker attacker;
        [SerializeField] private Animator animator;

        [SerializeField] private AnimationClip[] walkClips;
        [SerializeField] private AnimationClip[] attackClips;
        [SerializeField] private AnimationClip[] deathClips;

        [Header("State")]
        [SerializeField] private AnimatorOverrideController animController;

        private static readonly int AnimAttack = Animator.StringToHash("attack");
        private static readonly int AnimDie    = Animator.StringToHash("die");

        private void Awake()
        {
            animController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            Assert.IsNotNull(animController);

            animator.runtimeAnimatorController = animController;
        }

        private void OnEnable()
        {
            attacker.OnAttackPerformed += Attack_Performed;
            attacker.OnAttackProcced   += Attack_Procced;
        }

        private void OnDisable()
        {
            attacker.OnAttackPerformed -= Attack_Performed;
            attacker.OnAttackProcced   -= Attack_Procced;
        }

        private void Update()
        {
            int clipIndex = (int)attacker.AimDirection - 1;
            animController[walkClips[0]]  = walkClips[clipIndex];
            animController[deathClips[0]] = deathClips[clipIndex];

            bool preventTurn = (attacker.isPerformingAttack && !GameManager.Config.TurnWhileAttacking)
                               || (attacker.isRecovering && !GameManager.Config.TurnWhileRecovering);

            if (!preventTurn)
            {
                // swap to other animation before attack proc (same position)
                attacker.lockedDirection       = attacker.AimDirection;
                animController[attackClips[0]] = attackClips[clipIndex];
            }
            // else: original animation keeps playing normally
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
