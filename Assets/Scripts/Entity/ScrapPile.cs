using UnityEngine;

namespace Crabgame.Entity
{
    public class ScrapPile : Health
    {
        [Header("Scrap")]
        [SerializeField, Min(1)] private int scrapsGranted = 1;

        private void OnDestroy() =>
            GameManager.PlayerState.scrapAmount += scrapsGranted;
    }
}
