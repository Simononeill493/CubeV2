using System;
using System.Collections.Generic;

namespace CubeV2
{
    public class EntityTemplate
    {
        public string TemplateID { get; }
        private static int _sessionEntityCount;

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
            switch (SpecialTag)
            {
                case SpecialEntityTag.None:
                    return new Entity(TemplateID, _sessionEntityCount++.ToString(), Sprite) { Instructions = this.Instructions };
                case SpecialEntityTag.Goal:
                    return new GoalEntity(TemplateID, _sessionEntityCount++.ToString(), Sprite) { Instructions = this.Instructions };
                default:
                    throw new Exception("Generating unrecognized entity type");
            }
        }

        public enum SpecialEntityTag
        {
            None,
            Goal
        }
    }
}
