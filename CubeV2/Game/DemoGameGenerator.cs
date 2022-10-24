using System.Collections.Generic;

namespace CubeV2
{
    internal class DemoGameGenerator
    {
        public static Game CreateDemoGame(EntityTemplate player)
        {
            var game = new Game();

            game.SetTemplateTemplate(_createDemoTemplateTemplate(player));
            game.ResetBoardTemplate();
            game.ResetBoard();

            game.WinCondition = new GoalWinCondition(EntityDatabase.PlayerName);

            return game;
        }

        private static BoardTemplateTemplate _createDemoTemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new BoardTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };
            templateTemplate.Entities.Add(player);
            
            for (int i = 0; i < 40; i++)
            {
                templateTemplate.Entities.Add(EntityDatabase.GetTemplate(EntityDatabase.WallName));
            }

            for (int i = 0; i < 1; i++)
            {
                templateTemplate.Entities.Add(EntityDatabase.GetTemplate(EntityDatabase.GoalName));
            }

            return templateTemplate;
        }
    }
}
