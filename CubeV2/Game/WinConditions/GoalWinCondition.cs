using System.Linq;
using System.Reflection.Metadata;

namespace CubeV2
{
    public class GoalWinCondition : BoardWinCondition
    {
        string _entityTypeToCheck;

        public GoalWinCondition(string entityTypeToCheck)
        {
            _entityTypeToCheck = entityTypeToCheck;
        }

        public override bool Check(Board board)
        {
            if(!board.EntityTypes.ContainsKey(_entityTypeToCheck))
            {
                return false;
            }

            return board.EntityTypes[_entityTypeToCheck].Any(t=>t.HasTag(Config.GoalTag));
        }

    }
}
