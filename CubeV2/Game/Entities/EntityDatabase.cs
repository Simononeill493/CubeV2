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
        public const string AutoPlayerName = "AutoPlayer";
        public const string ManualPlayerName = "ManualPlayer";

        public const string RockName = "Rock";
        public const string EnergyRockName = "EnergyRock";

        public const string GoalName = "Goal";

        private static Dictionary<string, EntityTemplate> _masterList;
        public static IEnumerable<EntityTemplate> GetAll() => _masterList.Values;
        
        public static void Load()
        {
            _masterList = new Dictionary<string, EntityTemplate>();

            _masterList[ManualPlayerName] = new EntityTemplate(ManualPlayerName, EntityTemplate.SpecialEntityTag.ManualPlayer) { Sprite = DrawUtils.PlayerSprite };
            _masterList[ManualPlayerName].Instructions = new List<Instruction>();
            _masterList[ManualPlayerName].DefaultTags = new List<string>() { Config.PlayerTag };
            _masterList[ManualPlayerName].DefaultMaxEnergy = 200;

            _masterList[AutoPlayerName] = new EntityTemplate(AutoPlayerName) { Sprite = DrawUtils.PlayerSprite };
            _masterList[AutoPlayerName].Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Forward) };
            _masterList[AutoPlayerName].DefaultTags = new List<string>() { Config.PlayerTag };

            _masterList[RockName] = new EntityTemplate(RockName) { Sprite = DrawUtils.RockSprite };
            _masterList[EnergyRockName] = new EntityTemplate(EnergyRockName) { Sprite = DrawUtils.EnergyRockSprite };

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
