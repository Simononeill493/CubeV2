using System.Collections.Generic;

namespace CubeV2
{
    public class BoardTemplateTemplate
    {
        public int Width;
        public int Height;
        public List<EntityTemplate> Entities = new List<EntityTemplate>();

        public BoardTemplate GenerateTemplate()
        {
            var coords = Vector2Int.GetRandomUniqueCoords(Width, Height, Entities.Count);
            var entitiesDict = new Dictionary<Vector2Int, EntityTemplate>();

            for (int i = 0; i < Entities.Count; i++)
            {
                entitiesDict[coords[i]] = Entities[i];
            }

            var template = new BoardTemplate();
            template.Width = Width;
            template.Height = Height;
            template.Entities = entitiesDict;

            return template;
        }
    }
}
