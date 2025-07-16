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
        private Button        continueButton;

        private void Awake()
        {
            document.enabled = true;
            root             = document.rootVisualElement;

            root.visible = true;

#if UNITY_EDITOR
            // root.visible = false;
#endif

            continueButton         =  root.Q<Button>("ContinueButton");
            continueButton.clicked += ContinueButton_Clicked;
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

        private void ContinueButton_Clicked() =>
            GameManager.Instance.NextPhase();
    }
}
