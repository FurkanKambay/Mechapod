using Crabgame.Managers;
using UnityEngine;

namespace Crabgame.Entity
{
    public class ScrapPile : Health
    {
        public override void Die(Health source)
        {
            GameManager.PlayerState.scrapAmount += GameManager.Config.ScrapsPerPile;
            Destroy(gameObject);

            base.Die(source);
        }
    }
}
