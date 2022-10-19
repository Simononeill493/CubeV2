namespace CubeV2
{
    public abstract class BoardWinCondition
    {
        public abstract bool Check(Board board);

        public static BoardWinCondition None => new NoWinCondition();
    }
}
