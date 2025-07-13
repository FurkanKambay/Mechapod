using UnityEditor;
using UnityEngine;

namespace Shovel
{
    public class MinionMouse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Transform[] minions;

        private void Update()
        {
            foreach (Transform minion in minions)
            {
                // TODO: move towards mouse
            }
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.blue;
            Handles.DrawSolidDisc(input.AimPosition, Vector3.forward, 0.1f);
        }
    }
}
