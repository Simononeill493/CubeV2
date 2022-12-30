using System.Collections.Generic;

namespace CubeV2
{
    internal class DemoFindGoalGame : Game
    {
        private EntityTemplate _playerTemplate; 

        public DemoFindGoalGame (EntityTemplate player) : base()
        {
            _playerTemplate = player;

            SetTemplateTemplate(CreateTemplateTemplate());
            ResetBoardTemplate();
            ResetBoard();

            WinCondition = new FindGoalWinCondition(EntityDatabase.AutoPlayerName);
        }

        public override void CustomSetUpBoard(Board b)
        {
            foreach (var entity in b.ActiveEntities)
            {
                entity.GiveEnergy(entity.MaxEnergy);
            }            
        }

        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var templateTemplate = new FullyRandomTemplateTemplate() { Width = Config.GameGridDefaultWidth, Height = Config.GameGridDefaultHeight };
            templateTemplate.EntitiesRandomLocation.Add(_playerTemplate);

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
