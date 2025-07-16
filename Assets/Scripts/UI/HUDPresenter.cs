using UnityEngine;
using UnityEngine.UIElements;

namespace Crabgame.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;

        private VisualElement hpBar;

        private void Awake()
        {
            document.enabled = true;

            VisualElement root = document.rootVisualElement;
            hpBar = root.Q("GolemHPBar");

            root.visible  = true;

            root.dataSource = GameManager.Instance;

            GameManager_PhaseChange();
        }

        private void OnEnable()  => GameManager.Instance.OnPhaseChange += GameManager_PhaseChange;
        private void OnDisable() => GameManager.Instance.OnPhaseChange -= GameManager_PhaseChange;

        private void GameManager_PhaseChange() =>
            hpBar.visible = GameManager.Instance.IsNight;
    }
}
