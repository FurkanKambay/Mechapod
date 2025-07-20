using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Entity
{
    public class ScrapPile : Health
    {
        [Header("Scrap")]
        [SerializeField, Min(1)] private int scrapsGranted = 1;

        public override void Die(Health source)
        {
            GameManager.PlayerState.scrapAmount += scrapsGranted;
            Destroy(gameObject);

            base.Die(source);
        }
    }
}
