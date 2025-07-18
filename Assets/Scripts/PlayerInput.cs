using Crabgame.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crabgame
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference point;
        [SerializeField] private InputActionReference golemArm;

        public Vector2 AimPointScreen { get; private set; }
        public Vector2 AimPosition    { get; private set; }

        public bool WantsToUseArm { get; private set; }

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            // weird.
            point.asset.Enable();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsNight)
                return;

            AimPointScreen = point.action.ReadValue<Vector2>();
            AimPosition    = mainCamera.ScreenToWorldPoint(AimPointScreen);
            WantsToUseArm  = golemArm.action.triggered;
        }
    }
}
