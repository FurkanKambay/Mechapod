using UnityEngine;

namespace Shovel.Visual
{
    public class CrabAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;

        private static readonly int AnimDirection = Animator.StringToHash("direction");

        private void Update()
        {
            (float x, float y) signs = (Mathf.Sign(body.linearVelocityX), Mathf.Sign(body.linearVelocityY));

            animator.SetInteger(
                AnimDirection,
                signs switch
                {
                    (-1, 1)  => 1, // NW
                    (1, 1)   => 2, // NE
                    (1, -1)  => 3, // SE
                    (-1, -1) => 4, // SW
                    _        => 3  // default: SE
                }
            );
        }
    }
}
