using System;
using System.Collections.Generic;

namespace CubeV2
{
    public abstract class BoardTemplateTemplate
    {
        public int Width;
        public int Height;

        public abstract BoardTemplate GenerateTemplate();
    }

    public class BoardTest1TemplateTemplate : BoardTemplateTemplate
    {
        public Dictionary<Vector2Int, EntityTemplate> StaticEntities = new Dictionary<Vector2Int, EntityTemplate>();

        public override BoardTemplate GenerateTemplate()
        {
            var template = new BoardTemplate();
            template.Width = Width;
            template.Height = Height;
            template.Entities = new Dictionary<Vector2Int, EntityTemplate>();

            foreach(var kvp in StaticEntities)
            {
                template.Entities.Add(kvp.Key, kvp.Value);
            }

            return template;
        }
    }


    public class FullyRandomTemplateTemplate : BoardTemplateTemplate
    {
        public List<EntityTemplate> EntitiesRandomLocation = new List<EntityTemplate>();

        public override BoardTemplate GenerateTemplate()
        {
            var coords = Vector2Int.GetRandomUniqueCoords(Width, Height, EntitiesRandomLocation.Count);
            var entitiesDict = new Dictionary<Vector2Int, EntityTemplate>();

            for (int i = 0; i < EntitiesRandomLocation.Count; i++)
            {
                entitiesDict[coords[i]] = EntitiesRandomLocation[i];
            }

            var template = new BoardTemplate();
            template.Width = Width;
            template.Height = Height;
            template.Entities = entitiesDict;

            return template;
        }
    }
}
