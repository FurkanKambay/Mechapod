using UnityEngine;

namespace Shovel
{
    public class MinionMouse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Rigidbody2D[] minions;

        [Header("Config")]
        [SerializeField] private float moveSpeed;

        private void Update()
        {
            Vector2 targetPoint = input.AimPosition;
            float   deltaTime   = Time.deltaTime;

            foreach (Rigidbody2D minion in minions)
            {
                if (!minion)
                    continue;

                Vector2 newPosition = Vector2.MoveTowards(minion.position, targetPoint, moveSpeed * deltaTime);
                minion.MovePosition(newPosition);
            }
        }

        private void OnDrawGizmos()
        {
            // Handles.color = Color.blue;
            // Handles.DrawSolidDisc(input.AimPosition, Vector3.forward, 0.1f);
        }
    }
}
