using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CubeV2
{
    internal class BoardTest1Game : Game
    {
        private EntityTemplate _playerTemplate;

        public BoardTest1Game(EntityTemplate player) : base()
        {
            _playerTemplate = player;

            SetTemplateTemplate(CreateTemplateTemplate());
            ResetBoardTemplate();
            ResetBoard();
        }

        public override void CustomSetUpBoard(Board b)
        {
            foreach(var entity in b.ActiveEntities)
            {
                if (entity.TemplateID != EntityDatabase.PortalName)
                {
                    entity.GiveEnergy(Config.BoardTest1StartingEnergy);
                }
            }

            foreach(var energyRock in b.GetEntityByTemplate(EntityDatabase.EnergyRockName))
            {
                energyRock.SetEnergyToMax();
            }

            var portal = b.GetEntityByTemplate(EntityDatabase.PortalName).First();
            WinCondition = new EnergyWinCondition(portal, portal.MaxEnergy);

            GameInterface.DisplayText = "Goal: Supply 100 energy to the portal!";
        }


        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var lines = File.ReadAllLines("C:\\Users\\Simon\\Desktop\\CubeV2\\BigMap.txt");
            var width = lines[0].Length;
            var height = lines.Length;

            var templateTemplate = new BoardTest1TemplateTemplate() { Width = width, Height = height };
            templateTemplate.StaticEntities[new Vector2Int(61, 44)] = _playerTemplate;

            var rock = EntityDatabase.GetTemplate(EntityDatabase.RockName);
            var brokenRock = EntityDatabase.GetTemplate(EntityDatabase.BrokenRockName);
            var energyRock = EntityDatabase.GetTemplate(EntityDatabase.EnergyRockName);
            var portal = EntityDatabase.GetTemplate(EntityDatabase.PortalName);

            for (int y=0;y<lines.Length;y++)
            {
                for(int x = 0;x<lines[y].Length;x++)
                {
                    var position = new Vector2Int(x, y);
                    var tile = lines[y][x];

                    if(tile=='x')
                    {
                        if (RandomUtils.RandomNumber(10) == 0)
                        {
                            templateTemplate.StaticEntities[position] = brokenRock;
                        }
                        else if (RandomUtils.RandomNumber(25) == 0)
                        {
                            templateTemplate.StaticEntities[position] = energyRock;
                        }
                        else
                        {
                            templateTemplate.StaticEntities[position] = rock;
                        }
                    }
                    else if (tile == 'E')
                    {
                        templateTemplate.StaticEntities[position] = energyRock;
                    }
                    else if (RandomUtils.RandomNumber(3000) == 0)
                    {
                        templateTemplate.StaticEntities[position] = portal;
                    }


                }
            }

            templateTemplate.StaticEntities[new Vector2Int(5, 2)] = portal;

            /*for (int i = 0; i < 25; i++)
            {
                templateTemplate.Entities.Add(EntityDatabase.GetTemplate(EntityDatabase.WallName));
            }

            for (int i = 0; i < 1; i++)
            {
                templateTemplate.Entities.Add(EntityDatabase.GetTemplate(EntityDatabase.GoalName));
            }*/

            return templateTemplate;
        }
    }
}
