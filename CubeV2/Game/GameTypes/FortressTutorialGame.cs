using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CubeV2
{
    internal class FortressTurorialGame : Game
    {
        private EntityTemplate _playerTemplate;

        public FortressTurorialGame(EntityTemplate player) : base()
        {
            _playerTemplate = player;

            SetTemplateTemplate(CreateTemplateTemplate());
            ResetBoardTemplate();
            ResetBoard();
        }

        public override void CustomSetUpBoard(Board b)
        {
            GameInterface.DisplayText = "Fortress tutorial in development...";
        }


        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var lines = File.ReadAllLines(Config.FortressTutorialWorldPath);
            var width = lines[0].Length;
            var height = lines.Length;

            var templateTemplate = new FortressTutorialTemplateTemplate() { Width = width, Height = height };
            templateTemplate.StaticEntities[new Vector2Int(0, 12)] = _playerTemplate;

            var wall = EntityDatabase.Get(EntityDatabase.StoneWallName);
            var rock = EntityDatabase.Get(EntityDatabase.RockName);

            //var player = EntityDatabase.GetTemplate(EntityDatabase.ManualPlayerName);
            var respawner =  EntityDatabase.Get(EntityDatabase.RespawnerName);
            var craftingTable= EntityDatabase.Get(EntityDatabase.CraftingTableName);
            var turret = EntityDatabase.Get(EntityDatabase.TurretName);
            var goal = EntityDatabase.Get(EntityDatabase.GoalName);
            var laserFlower = EntityDatabase.Get(EntityDatabase.LaserFlowerName);
            var pickaxeFlower = EntityDatabase.Get(EntityDatabase.PickaxeFlowerName);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var position = new Vector2Int(x, y);
                    var tile = lines[y][x];

                    if (tile == 'W')//Wall
                    {
                        templateTemplate.StaticEntities[position] = wall;
                    }
                    else if (tile == 'R')//Rock
                    {
                        templateTemplate.StaticEntities[position] = rock;
                    }
                    /*else if (tile == 'P')//Player
                    {
                        templateTemplate.StaticEntities[position] = player;
                    }*/
                    else if (tile == 'T')//Respawner
                    {
                        templateTemplate.StaticEntities[position] = respawner;
                    }
                    else if (tile == 'C')//Crafting table
                    {
                        templateTemplate.StaticEntities[position] = craftingTable;
                    }
                    else if (tile == 'E')//Turret
                    {
                        templateTemplate.StaticEntities[position] = turret;
                    }
                    else if (tile == 'G')//Goal
                    {
                        templateTemplate.StaticEntities[position] = goal;
                    }
                    else if (tile == 'L')//Laser Flower
                    {
                        templateTemplate.StaticEntities[position] = laserFlower;
                    }
                    else if (tile == 'M')//Pickaxe flower
                    {
                        templateTemplate.StaticEntities[position] = pickaxeFlower;
                    }


                }
            }
            return templateTemplate;
        }
    }
}
