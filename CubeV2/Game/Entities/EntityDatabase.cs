using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class EntityDatabase
    {
        public const string PlayerName = "Player";
        public const string WallName = "Wall";
        public const string GoalName = "Goal";

        private static Dictionary<string, EntityTemplate> _masterList;
        public static IEnumerable<EntityTemplate> GetAll() => _masterList.Values;
        
        public static void Load()
        {
            _masterList = new Dictionary<string, EntityTemplate>();

            _masterList[PlayerName] = new EntityTemplate(PlayerName) { Sprite = DrawUtils.PlayerSprite };
            _masterList[PlayerName].Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Forward) };

            _masterList[WallName] = new EntityTemplate(WallName) { Sprite = DrawUtils.WallSprite };
            _masterList[GoalName] = new EntityTemplate(GoalName, EntityTemplate.SpecialEntityTag.Goal) { Sprite = DrawUtils.GoalSprite };
        }

        public static EntityTemplate GetTemplate(string id)
        {
            if(!_masterList.ContainsKey(id))
            {
                Console.WriteLine("Error: This entity template does not exist.");
                return null;
            }

            return _masterList[id];
        }
    }
}
