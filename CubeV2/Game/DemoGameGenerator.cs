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

            game.WinCondition = new GoalWinCondition(player.Id);

            return game;
        }

        public static EntityTemplate CreateDemoPlayer()
        {
            var player = new EntityTemplate("Player") { Sprite = DrawUtils.PlayerSprite };
            player.Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Backward) };

            return player;
        }

        private static BoardTemplateTemplate _createDemoTemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new BoardTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };
            templateTemplate.Entities.Add(player);
            
            for (int i = 0; i < 40; i++)
            {
                templateTemplate.Entities.Add(new EntityTemplate("Wall_" + i) { Sprite = DrawUtils.WallSprite });
            }

            templateTemplate.Entities.Add(new EntityTemplate("Goal", EntityTemplate.SpecialEntityTag.Goal) { Sprite = DrawUtils.GoalSprite });

            return templateTemplate;
        }
    }
}
