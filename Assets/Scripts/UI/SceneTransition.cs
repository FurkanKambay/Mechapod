using System.Collections;
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
        [SerializeField] private bool showTransition;
        [SerializeField] private bool showGameOver;
        [SerializeField] private bool showGameSuccess;

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
            SetTransition(false);
        }

        public IEnumerator StartTransition()
        {
            SetTransition(true);
            yield return new WaitForSecondsRealtime(transitionDuration);
        }

        public IEnumerator EndTransition()
        {
            SetTransition(false);
            yield return new WaitForSecondsRealtime(transitionDuration);
        }

        public void GameSuccess(bool show)
        {
            SetTransition(show);
            SetGameSuccess(show);
        }

        public void GameOver(bool show)
        {
            SetTransition(show);
            SetGameOver(show);
        }

        private void SetTransition(bool value)
        {
            showTransition                = value;
            blackBackground.style.opacity = showTransition ? 1f : 0f;
        }

        private void SetGameOver(bool value)
        {
            showGameOver          = value;
            gameOverPanel.visible = showGameOver;
        }

        private void SetGameSuccess(bool value)
        {
            showGameSuccess          = value;
            gameSuccessPanel.visible = showGameSuccess;
            // gameSuccessPanel.style.opacity = value ? 1f : 0f;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying || blackBackground == null)
                return;

            SetTransition(showTransition);
            SetGameOver(showGameOver);
            SetGameSuccess(showGameSuccess);
        }
#endif
    }
}
