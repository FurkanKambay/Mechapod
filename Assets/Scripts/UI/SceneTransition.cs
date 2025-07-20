using UnityEngine;
using UnityEngine.UIElements;

namespace Crabgame.UI
{
    public class SceneTransition : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;

        [Header("Config")]
        [SerializeField] private float transitionDuration = 0.2f;

        [Header("State")]
        [SerializeField] private bool isTransitioning;

        private VisualElement root;
        private VisualElement blackBackground;
        private VisualElement gameOverPanel;
        private VisualElement gameSuccessPanel;

        private void Awake()
        {
            root             = document.rootVisualElement;
            blackBackground  = root.Q<VisualElement>("black");
            gameOverPanel    = root.Q<VisualElement>("game-over-panel");
            gameSuccessPanel = root.Q<VisualElement>("game-success-panel");

            SetGameOver(false);
            SetGameSuccess(false);
        }

        public void SetTransition(bool value)
        {
            isTransitioning               = value;
            blackBackground.style.opacity = isTransitioning ? 1f : 0f;
        }

        public void SetGameOver(bool value) =>
            gameOverPanel.visible = value;

        public void SetGameSuccess(bool value) =>
            gameSuccessPanel.visible = value;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetTransition(isTransitioning);
        }
#endif
    }
}
