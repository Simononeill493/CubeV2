using System.Linq;
using System.Reflection.Metadata;

namespace CubeV2
{
    public class FindGoalWinCondition : BoardWinCondition
    {
        string _templateToCheck;

        public FindGoalWinCondition(string entityTypeToCheck)
        {
            _templateToCheck = entityTypeToCheck;
        }

        public override bool Check(Board board)
        {
            var listToCheck = board.GetEntityByTemplate(_templateToCheck);

            return listToCheck.Any(t => t.HasTag(Config.GoalTag));
        }

    }
}
