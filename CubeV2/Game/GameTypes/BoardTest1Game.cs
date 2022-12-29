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

            var portal = CurrentBoard.GetEntityByTemplate(EntityDatabase.PortalName).First();
            WinCondition = new EnergyWinCondition(portal, portal.MaxEnergy);
        }

        public override void CustomSetUpBoard(Board b)
        {
            foreach(var entity in b.ActiveEntities)
            {
                entity.GiveEnergy(Config.BoardTest1StartingEnergy);
            }
        }


        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var templateTemplate = new BoardTest1TemplateTemplate() { Width = Config.GameGridDefaultWidth, Height = Config.GameGridDefaultHeight };
            templateTemplate.StaticEntities[new Vector2Int(7, 7)] = _playerTemplate;

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
