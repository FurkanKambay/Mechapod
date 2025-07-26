using Crabgame.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crabgame.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;
        [SerializeField] private PlayerInput playerInput;

        private Button laserButton;

        private void Awake()
        {
            document.enabled = true;

            VisualElement root = document.rootVisualElement;
            laserButton = root.Q<Button>("LaserButton");

            root.visible    = true;
            root.dataSource = GameManager.Instance;

            laserButton.clicked += LaserButton_Clicked;
        }

        private void LaserButton_Clicked() =>
            playerInput.RequestToUseArm();
    }
}
