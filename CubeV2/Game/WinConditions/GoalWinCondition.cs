using System.Linq;
using System.Reflection.Metadata;

namespace CubeV2
{
    public class FindGoalWinCondition : BoardWinCondition
    {
        string _templateToCheck;
        //We can't test a specific entity because that tests a SPECIFIC goal. We want to see if ANY player has reached ANY goal. 

        public FindGoalWinCondition(string entityTypeToCheck)
        {
            _templateToCheck = entityTypeToCheck;
        }

        public override bool Check(Board board)
        {
            return board.GetActiveEntityByTag(Config.PlayerTag).Any(t => t.HasTag(Config.CollectedGoalTag));
        }

    }
}
