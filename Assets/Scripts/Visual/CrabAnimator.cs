using System;
using UnityEngine;

namespace Shovel.Visual
{
    public class CrabAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;

        [Header("State")]
        [SerializeField] private Vector2 velocity;

        private static readonly int AnimDirection = Animator.StringToHash("direction");

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
    }
}
