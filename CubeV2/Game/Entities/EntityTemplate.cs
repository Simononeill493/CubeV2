using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class EntityTemplate
    {
        public string TemplateID { get; }
        private static int _sessionEntityCount;

        public List<string> DefaultTags = new List<string>();
        public int DefaultMaxEnergy = -1;

        public SpecialEntityTag SpecialTag;

        public string Sprite;
       

        public List<Instruction> Instructions = new List<Instruction>();

        public EntityTemplate(string id, SpecialEntityTag specialTag = SpecialEntityTag.None)
        {
            TemplateID = id;
            SpecialTag = specialTag;
        }

        public Entity GenerateEntity()
        {
            Entity entity;

            switch (SpecialTag)
            {
                case SpecialEntityTag.None:
                    entity = new Entity(TemplateID, _sessionEntityCount++.ToString(), Sprite);
                    break;
                case SpecialEntityTag.Goal:
                    entity = new GoalEntity(TemplateID, _sessionEntityCount++.ToString(), Sprite);
                    break;
                case SpecialEntityTag.ManualPlayer:
                    entity = new ManualPlayerEntity(TemplateID, _sessionEntityCount++.ToString(), Sprite);
                    break;
                default:
                    throw new Exception("Generating unrecognized entity type");
            }

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

            entity.CurrentEnergy = entity.MaxEnergy;
            return entity;
        }

        public enum SpecialEntityTag
        {
            None,
            Goal,
            ManualPlayer,
        }
    }
}
