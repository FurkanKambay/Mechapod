using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Shovel.UI
{
    public class GaragePresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;

        private VisualElement root;
        private Button        continueButton;

        private void Awake()
        {
            root         = document.rootVisualElement;
            root.visible = false;

            continueButton         =  root.Q<Button>("ContinueButton");
            continueButton.clicked += ContinueButton_Clicked;
        }

        private void ContinueButton_Clicked()
        {
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
                root.visible = !root.visible;
#endif
        }
    }
}
