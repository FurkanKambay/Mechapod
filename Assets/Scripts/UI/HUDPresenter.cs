using UnityEngine;
using UnityEngine.UIElements;

namespace Shovel.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument document;

        private void Awake()
        {
            VisualElement root = document.rootVisualElement;

            document.enabled = true;
            root.visible     = true;

            root.dataSource = GameManager.Instance;
        }
    }
}
