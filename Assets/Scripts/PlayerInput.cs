using UnityEngine;
using UnityEngine.InputSystem;

namespace Shovel
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference point;

        public Vector2 AimPointScreen { get; private set; }
        public Vector2 AimPosition    { get; private set; }

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            // weird
            point.asset.Enable();
        }

        private void Update()
        {
            AimPointScreen = point.action.ReadValue<Vector2>();
            AimPosition    = mainCamera.ScreenToWorldPoint(AimPointScreen);
        }
    }
}
