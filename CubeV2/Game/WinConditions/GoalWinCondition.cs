using System.Linq;
using System.Reflection.Metadata;

namespace CubeV2
{
    public class GoalWinCondition : BoardWinCondition
    {
        string _templateToCheck;

        public GoalWinCondition(string entityTypeToCheck)
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
