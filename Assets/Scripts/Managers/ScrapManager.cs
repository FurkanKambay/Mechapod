namespace Crabgame.Managers
{
    public class ScrapManager : SpawnerManager
    {
        protected override float MoveSpeed             => 0;
        protected override float RandomAttackOffsetMax => 0;

        private void Awake()
        {
            RespawnAll(GameManager.Tonight.Waves[0].ScrapAmount);
        }
    }
}
