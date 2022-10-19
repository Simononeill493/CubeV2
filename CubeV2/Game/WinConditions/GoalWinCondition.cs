namespace CubeV2
{
    public class GoalWinCondition : BoardWinCondition
    {
        string _toEnterGoalId;

        public GoalWinCondition(string toEnterGoalId)
        {
            _toEnterGoalId = toEnterGoalId;
        }

        public override bool Check(Board board)
        {
            return board.Entities[_toEnterGoalId].HasTag(Config.GoalTag);
        }

    }
}
