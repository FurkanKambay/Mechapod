using Crabgame.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Crabgame.UI
{
    public class GaragePresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;

        private VisualElement root;

        private Button continueButton;
        private Button armUpgradeButton;

        private void Awake()
        {
            document.enabled = true;
            root             = document.rootVisualElement;
            root.dataSource  = GameManager.Instance;

            root.visible = true;

#if UNITY_EDITOR
            // root.visible = false;
#endif

            continueButton   = root.Q<Button>("ContinueButton");
            armUpgradeButton = root.Q<Button>("ArmUpgradeButton");

            continueButton.clicked   += ContinueButton_Clicked;
            armUpgradeButton.clicked += ArmUpgradeButton_Clicked;
        }

        private void OnEnable()  => GameManager.Instance.OnPhaseChange += GameManager_PhaseChange;
        private void OnDisable() => GameManager.Instance.OnPhaseChange -= GameManager_PhaseChange;

        private void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
                root.visible = !root.visible;
#endif
        }

        private void GameManager_PhaseChange() =>
            root.visible = !GameManager.Instance.IsNight;

        private static void ContinueButton_Clicked() =>
            GameManager.Instance.NextPhase();

        private static void ArmUpgradeButton_Clicked() =>
            GameManager.PlayerState.BuyArm();
    }
}
