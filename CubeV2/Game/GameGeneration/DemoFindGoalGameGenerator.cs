using System.Collections.Generic;

namespace CubeV2
{
    internal class DemoFindGoalGameGenerator
    {
        public static Game CreateDemoFindGoalGame(EntityTemplate player)
        {
            var game = new Game();

            game.SetTemplateTemplate(_createDemoFindGoalTemplateTemplate(player));
            game.ResetBoardTemplate();
            game.ResetBoard();

            game.WinCondition = new FindGoalWinCondition(EntityDatabase.AutoPlayerName);

            return game;
        }

        private static BoardTemplateTemplate _createDemoFindGoalTemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new FullyRandomTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };
            templateTemplate.EntitiesRandomLocation.Add(player);

            for (int i = 0; i < 25; i++)
            {
                templateTemplate.EntitiesRandomLocation.Add(EntityDatabase.GetTemplate(EntityDatabase.RockName));
            }

            for (int i = 0; i < 1; i++)
            {
                templateTemplate.EntitiesRandomLocation.Add(EntityDatabase.GetTemplate(EntityDatabase.GoalName));
            }

            return templateTemplate;
        }
    }
}
