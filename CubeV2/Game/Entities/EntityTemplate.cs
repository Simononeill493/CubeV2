using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class EntityTemplate
    {
        public string TemplateID { get; }
        protected static int _sessionEntityCount;

        public List<string> DefaultTags = new List<string>();
        public int DefaultMaxEnergy = -1;

        public string DefaultSprite;

        public List<Instruction> Instructions = null;

        public EntityTemplate(string id)
        {
            TemplateID = id;
        }

        public Entity GenerateEntity()
        {
            var entity = _createEntity();
            entity.Instructions = this.Instructions;

            if (DefaultTags.Any())
            {
                entity.Tags = new List<string>(DefaultTags);
            }

            if (DefaultMaxEnergy > -1)
            {
                entity.MaxEnergy = DefaultMaxEnergy;
            }
            else
            {
                entity.MaxEnergy = Config.GlobalDefaultMaxEnergy;
            }

            return entity;
        }

        protected virtual Entity _createEntity()
        {
            return new Entity(TemplateID, _sessionEntityCount++.ToString(), DefaultSprite);
        }
    }

    public class GoalTemplate : EntityTemplate
    {
        public GoalTemplate(string id) : base(id){ DefaultSprite = DrawUtils.GoalSprite; }

        protected override Entity _createEntity()
        {
            return new GoalEntity(TemplateID, _sessionEntityCount++.ToString(), DefaultSprite); ;
        }
    }

    public class ManualPlayerTemplate : EntityTemplate
    {
        public ManualPlayerTemplate(string id) : base(id) { DefaultSprite = DrawUtils.PlayerSprite; }

        protected override Entity _createEntity()
        {
            return new ManualPlayerEntity(TemplateID, _sessionEntityCount++.ToString(), DefaultSprite);
        }
    }

    public class CollectableEntityTemplate : EntityTemplate
    {
        public CollectableEntityTemplate(string id) : base(id) { }

        protected override Entity _createEntity()
        {
            return new CollectableEntity(TemplateID, _sessionEntityCount++.ToString(), DefaultSprite);
        }
    }


    public class RockTemplate : EntityTemplate
    {
        public RockTemplate(string id) : base(id) { }

        protected override Entity _createEntity()
        {
            var sprite = DrawUtils.RockSprite1;

            if(RandomUtils.RandomNumber(5)==0)
            {
                sprite = DrawUtils.RockSprite2;
            }
            return new RockEntity(TemplateID, _sessionEntityCount++.ToString(), sprite);
        }
    }

    public class EnergyRockTemplate : EntityTemplate
    {
        public EnergyRockTemplate(string id) : base(id) { DefaultSprite = DrawUtils.EnergyRockSprite; }

        protected override Entity _createEntity()
        {
            return new RockEntity(TemplateID, _sessionEntityCount++.ToString(), DefaultSprite);
        }
    }


}
