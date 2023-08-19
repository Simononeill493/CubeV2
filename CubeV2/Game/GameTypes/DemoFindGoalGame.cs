using System;
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

        public override void OnSetBoard(Board b)
        {
            foreach (var entity in b.ActiveEntities)
            {
                entity.GiveEnergy(entity.MaxEnergy);
            }            
        }

        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var templateTemplate = new FullyRandomTemplateTemplate() { Width = 24, Height = 16 };
            templateTemplate.EntitiesRandomLocation.Add(_playerTemplate);

            for (int i = 0; i < 25; i++)
            {
                templateTemplate.EntitiesRandomLocation.Add(EntityDatabase.Get(EntityDatabase.RockName));
            }

            for (int i = 0; i < 1; i++)
            {
                templateTemplate.EntitiesRandomLocation.Add(EntityDatabase.Get(EntityDatabase.GoalName));
            }

            return templateTemplate;
        }

        public override void RespawnPlayer()
        {
            throw new NotImplementedException();
        }

        public override void OnPlayerDeath()
        {
            throw new NotImplementedException();
        }



    }



}
