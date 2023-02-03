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

        public const string Ally1Name = "Ally1";
        public const string Ally2Name = "Ally2";
        public const string Ally3Name = "Ally3";
        public const string Ally4Name = "Ally4";
        public const string Ally5Name = "Ally5";

        public const string RockName = "Rock";
        public const string EnergyRockName = "EnergyRock";
        public const string BrokenRockName = "BrokenRock";

        public const string GoalName = "Goal";
        public const string PortalName = "Portal";

        private static Dictionary<string, EntityTemplate> _masterList;
        public static List<EntityTemplate> GetAll() => _masterList.Values.ToList();

        
        public static void Load()
        {
            _masterList = new Dictionary<string, EntityTemplate>();

            _masterList[ManualPlayerName] = new ManualPlayerTemplate(ManualPlayerName);
            _masterList[ManualPlayerName].Instructions = new List<Instruction>();
            _masterList[ManualPlayerName].DefaultTags = new List<string>() { Config.PlayerTag, Config.IndestructibleTag };
            _masterList[ManualPlayerName].DefaultMaxEnergy = 1000;

            _masterList[AutoPlayerName] = new EntityTemplate(AutoPlayerName) { DisplaySprite = DrawUtils.PlayerSprite };
            _masterList[AutoPlayerName].Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Forward) };
            _masterList[AutoPlayerName].DefaultTags = new List<string>() { Config.PlayerTag };


            _masterList[Ally1Name] = new EntityTemplate(Ally1Name) { DisplaySprite = DrawUtils.Ally1Sprite, Instructions = new List<Instruction>(), DefaultMaxEnergy = 10000 };
            _masterList[Ally1Name].DefaultTags = new List<string>() { Config.IndestructibleTag };

            _masterList[Ally2Name] = new EntityTemplate(Ally2Name) { DisplaySprite = DrawUtils.Ally2Sprite, Instructions = new List<Instruction>(), DefaultMaxEnergy = 50 };
            _masterList[Ally3Name] = new EntityTemplate(Ally3Name) { DisplaySprite = DrawUtils.Ally3Sprite, Instructions = new List<Instruction>(), DefaultMaxEnergy = 50 };
            _masterList[Ally4Name] = new EntityTemplate(Ally4Name) { DisplaySprite = DrawUtils.Ally4Sprite, Instructions = new List<Instruction>(), DefaultMaxEnergy = 50 };
            _masterList[Ally5Name] = new EntityTemplate(Ally5Name) { DisplaySprite = DrawUtils.Ally5Sprite, Instructions = new List<Instruction>(), DefaultMaxEnergy = 50 };

            _masterList[RockName] = new RockTemplate(RockName);
            _masterList[RockName].DefaultMaxEnergy = 0;

            _masterList[BrokenRockName] = new CollectableEntityTemplate(BrokenRockName) { DisplaySprite = DrawUtils.BrokenRockSprite };

            _masterList[EnergyRockName] = new EnergyRockTemplate(EnergyRockName);
            _masterList[EnergyRockName].DefaultMaxEnergy = 250;

            _masterList[GoalName] = new GoalTemplate(GoalName);

            _masterList[PortalName] = new EntityTemplate(PortalName) { DisplaySprite = DrawUtils.PortalSprite };
            _masterList[PortalName].DefaultTags = new List<string>() { Config.IndestructibleTag };
            _masterList[PortalName].DefaultMaxEnergy = 100;
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
