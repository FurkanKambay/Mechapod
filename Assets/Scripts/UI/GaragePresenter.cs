using System.Collections;
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
        [SerializeField] private SceneTransition sceneTransition;

        private VisualElement root;

        private Button continueButton;
        private Button armUpgradeButton;
        private Button buyMinionsButton;

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
            buyMinionsButton = root.Q<Button>("MinionUpgradeButton");

            continueButton.clicked   += ContinueButton_Clicked;
            armUpgradeButton.clicked += ArmUpgradeButton_Clicked;
            buyMinionsButton.clicked += BuyMinionsButton_Clicked;
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
            StartCoroutine(UpdateUI());

        private IEnumerator UpdateUI()
        {
            root.visible = !GameManager.Instance.IsNight;
            yield return null;
        }

        private void ContinueButton_Clicked() =>
            StartCoroutine(GameManager.Instance.NextPhase());

        private static void ArmUpgradeButton_Clicked() =>
            GameManager.PlayerState.BuyArm();

        private static void BuyMinionsButton_Clicked() =>
            GameManager.PlayerState.BuyMinions();
    }
}
