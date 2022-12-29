using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CubeV2
{
    internal class BoardTest1GameGenerator
    {
        public static Game CreateGemoBoardTest1Game(EntityTemplate player)
        {
            var game = new Game();

            game.SetTemplateTemplate(_createDemoBoardTest1TemplateTemplate(player));
            game.ResetBoardTemplate();
            game.ResetBoard();

            var portal = game.CurrentBoard.GetEntityByTemplate(EntityDatabase.PortalName).First();
            portal.CurrentEnergy = 0;

            game.WinCondition = new EnergyWinCondition(portal,portal.MaxEnergy);

            return game;
        }

        private static BoardTemplateTemplate _createDemoBoardTest1TemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new BoardTest1TemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };
            templateTemplate.StaticEntities[new Vector2Int(7, 7)] = player;

            var rock = EntityDatabase.GetTemplate(EntityDatabase.RockName);
            var energyRock = EntityDatabase.GetTemplate(EntityDatabase.EnergyRockName);

            var lines = File.ReadAllLines("C:\\Users\\Simon\\Desktop\\CubeV2\\SampleMapSmall.txt");
            for(int y=0;y<lines.Length;y++)
            {
                for(int x = 0;x<lines[y].Length;x++)
                {
                    var position = new Vector2Int(x, y);
                    var tile = lines[y][x];

                    if(tile=='x')
                    {
                        templateTemplate.StaticEntities[position] = rock;
                    }
                    else if (tile == 'E')
                    {
                        templateTemplate.StaticEntities[position] = energyRock;
                    }

                }
            }

            templateTemplate.StaticEntities[new Vector2Int(5, 2)] = EntityDatabase.GetTemplate(EntityDatabase.PortalName);

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
