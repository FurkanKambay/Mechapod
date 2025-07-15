using System;
using Shovel.Audio;
using Shovel.Entity;
using UnityEngine;

namespace Shovel.Visual
{
    public class CrabAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Attacker attacker;
        [SerializeField] private Animator animator;

        [Header("State")]
        [SerializeField] private Vector2 velocity;

        private static readonly int AnimDirection = Animator.StringToHash("direction");
        private static readonly int AnimAttack    = Animator.StringToHash("attack");
        private static readonly int AnimDie       = Animator.StringToHash("die");

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

            int signX = Math.Sign(velocity.x);
            int signY = Math.Sign(velocity.y);

            int direction = (signX, signY) switch
            {
                (-1, 1)  => 1, // NW
                (1, 1)   => 2, // NE
                (1, -1)  => 3, // SE
                (-1, -1) => 4, // SW
                _        => 3  // default: SE
            };

            animator.SetInteger(AnimDirection, direction);
        }

        private void Attack_Performed()
        {
            animator.SetTrigger(AnimAttack);
        }

        private void AttackHitFrame() =>
            attacker.ProcAttack();

        private static void Attack_Procced(bool hitTarget)
        {
            if (hitTarget)
                AudioPlayer.Instance.crabAttack.PlayOneShot();
            else
                AudioPlayer.Instance.crabAttackMiss.PlayOneShot();
        }
    }
}
