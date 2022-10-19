namespace CubeV2
{
    public class NoWinCondition : BoardWinCondition
    {
        public override bool Check(Board board) => false;
    }
}
