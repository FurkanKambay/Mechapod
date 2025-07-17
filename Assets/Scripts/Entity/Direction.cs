namespace Crabgame.Entity
{
    public enum Direction
    {
        NorthWest = 1,
        NorthEast,
        SouthEast,
        SouthWest
    }

    public static class DirectionExtensions
    {
        public static float AttackAngle(this Direction direction)
        {
            return direction switch
            {
                Direction.NorthWest => 153.435f,
                Direction.NorthEast => 26.565f,
                Direction.SouthEast => -26.565f,
                Direction.SouthWest => -153.435f,
                _                   => 0
            };
        }
    }
}
