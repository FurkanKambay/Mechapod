namespace Shovel.Entity
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
                Direction.NorthWest => 143.13f,
                Direction.NorthEast => 26.565f,
                Direction.SouthEast => -26.565f,
                Direction.SouthWest => -143.13f,
                _                   => 0
            };
        }
    }
}
