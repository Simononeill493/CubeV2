using SAME;
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
            template.EntitiesToPlace = new Dictionary<Vector2Int, EntityTemplate>();

            foreach (var kvp in StaticEntities)
            {
                template.EntitiesToPlace.Add(kvp.Key, kvp.Value);
            }

            return template;
        }
    }

    public class FortressTutorialTemplateTemplate : BoardTemplateTemplate
    {
        public Dictionary<Vector2Int, EntityTemplate> StaticEntities = new Dictionary<Vector2Int, EntityTemplate>();
        public Dictionary<Vector2Int, string> GroundSprites = new Dictionary<Vector2Int, string>();

        public override BoardTemplate GenerateTemplate()
        {
            var template = new FortressTutorialTemplate();
            template.Width = Width;
            template.Height = Height;
            template.GroundSprites = new Dictionary<Vector2Int, string>(GroundSprites);
            template.EntitiesToPlace = new Dictionary<Vector2Int, EntityTemplate>(StaticEntities);

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
            template.EntitiesToPlace = entitiesDict;

            return template;
        }
    }
}
