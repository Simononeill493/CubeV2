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
        public List<(int index,IVariableType variableType,object contents)> DefaultVariables = new List<(int,IVariableType, object)>();

        public bool CanBeDamaged = true;
        public int DefaultMaxHealth = 1;
        public int DefaultMaxEnergy = -1;
        public int DefaultUpdateRate = 1;

        public string DisplaySprite;

        public List<Instruction[]> Instructions { get; private set; } = null;

        public EntityTemplate(string id)
        {
            TemplateID = id;
        }

        public Entity GenerateEntity()
        {
            var entity = _createEntity();
            entity.Instructions = this.Instructions;
            entity.UpdateRate = DefaultUpdateRate;

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
            entity.CanBeDamaged = CanBeDamaged;
            entity.MaxHealth = DefaultMaxHealth;
            entity.SetHealthToMax();

            _setDefaultVariables(entity);


            return entity;
        }

        public void AddDefaultVariable(int index,IVariableType variableType,object contents)
        {
            DefaultVariables.Add((index, variableType, contents));
        }

        private void _setDefaultVariables(Entity e)
        {
            foreach(var variable in DefaultVariables)
            {
                switch(variable.variableType)
                {
                    case IVariableType.Integer:
                        e.Variables[variable.index] = new IntegerVariable((int)variable.contents);
                        continue;
                    default:
                        throw new NotImplementedException("Creating a default variable of type " + variable.variableType + " is not implemented");
                }
            }

        }

        protected virtual Entity _createEntity()
        {
            return new Entity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite);
        }

        public void MakeInstructable()
        {
            Instructions = new List<Instruction[]> { new Instruction[Config.EntityMaxInstructionsPerSet] };
        }
    }

    public class GoalTemplate : EntityTemplate
    {
        public GoalTemplate(string id) : base(id){ DisplaySprite = DrawUtils.GoalSprite; }

        protected override Entity _createEntity()
        {
            return new GoalEntity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite); ;
        }
    }

    public class ManualPlayerTemplate : EntityTemplate
    {
        public ManualPlayerTemplate(string id) : base(id) { DisplaySprite = DrawUtils.PlayerSprite; }

        protected override Entity _createEntity()
        {
            return new ManualPlayerEntity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite);
        }
    }

    public class CollectableEntityTemplate : EntityTemplate
    {
        public CollectableEntityTemplate(string id) : base(id) { }

        protected override Entity _createEntity()
        {
            return new CollectableEntity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite);
        }
    }

    public class HarvestableEntityTemplate : EntityTemplate
    {
        public int HarvestCount; 
        public HarvestableEntityTemplate(string id,int harvestCount) : base(id) 
        {
            HarvestCount = harvestCount;
            CanBeDamaged = false;
        }

        protected override Entity _createEntity()
        {
            return new HarvestableEntity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite,HarvestCount);
        }
    }



    public class RockTemplate : EntityTemplate
    {
        public RockTemplate(string id) : base(id) { DisplaySprite = DrawUtils.RockSprite1; }

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
        public EnergyRockTemplate(string id) : base(id) { DisplaySprite = DrawUtils.EnergyRockSprite; }

        protected override Entity _createEntity()
        {
            return new RockEntity(TemplateID, _sessionEntityCount++.ToString(), DisplaySprite);
        }
    }


}
