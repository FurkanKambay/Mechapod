using System;
using Shovel.Audio;
using Shovel.Entity;
using UnityEngine;
using UnityEngine.Assertions;

namespace Shovel.Visual
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

        [SerializeField] private Vector2   velocity;
        [SerializeField] private Direction lastDirection;

        [SerializeField] private bool isPerformingAttack;
        [SerializeField] private bool isRecovering;

        private static readonly int AnimDirection = Animator.StringToHash("direction");
        private static readonly int AnimAttack    = Animator.StringToHash("attack");
        private static readonly int AnimDie       = Animator.StringToHash("die");

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
            velocity = body.linearVelocity;

            Direction direction = (Math.Sign(velocity.x), Math.Sign(velocity.y)) switch
            {
                (-1, 1)  => Direction.NorthWest,
                (1, 1)   => Direction.NorthEast,
                (1, -1)  => Direction.SouthEast,
                (-1, -1) => Direction.SouthWest,
                _        => Direction.SouthEast
            };

            int clipIndex = (int)direction - 1;
            animController[walkClips[0]]  = walkClips[clipIndex];
            animController[deathClips[0]] = deathClips[clipIndex];

            bool isTurningAllowed = !isPerformingAttack || GameManager.Config.TurnWhileAttacking;

            if (!isRecovering && isTurningAllowed)
            {
                lastDirection                  = direction;
                animController[attackClips[0]] = attackClips[clipIndex];
            }

            animator.SetInteger(AnimDirection, (int)lastDirection);
        }

        private void Attack_Performed()
        {
            isPerformingAttack = true;
            animator.SetTrigger(AnimAttack);
        }

        private void Anim_AttackProc() =>
            attacker.ProcAttack(lastDirection);

        private void Anim_AttackDone() =>
            isRecovering = false;

        private void Attack_Procced(bool hitTarget)
        {
            isPerformingAttack = false;
            isRecovering       = true;

            if (hitTarget)
                AudioPlayer.Instance.crabAttack.PlayOneShot();
            else
                AudioPlayer.Instance.crabAttackMiss.PlayOneShot();
        }
    }
}
