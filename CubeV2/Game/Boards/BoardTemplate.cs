using System;
using System.Collections.Generic;

namespace CubeV2
{
    public class BoardTemplate
    {
        public Dictionary<Vector2Int, EntityTemplate> Entities = new Dictionary<Vector2Int, EntityTemplate>();
        public int Width;
        public int Height;

        public Board GenerateBoard()
        {
            var board = new Board(Width, Height);

            var entitiesGenerated = new List<(Entity,Vector2Int)>();
            foreach (var entity in Entities)
            {
                entitiesGenerated.Add((entity.Value.GenerateEntity(), entity.Key));
            }

            PrepareEntities(entitiesGenerated);

            foreach(var entity in entitiesGenerated)
            {
                board.TryAddEntityToBoard(entity.Item1, entity.Item2);
            }

            return board;
        }

        public virtual void PrepareEntities(List<(Entity e, Vector2Int location)> entities) { }
    }

    public class FortressTutorialTemplate : BoardTemplate
    {
        public override void PrepareEntities(List<(Entity e, Vector2Int location)> entities)
        {
            var shortRangeTurrets = new List<Vector2Int>();
            shortRangeTurrets.Add(new Vector2Int(86, 20));
            shortRangeTurrets.Add(new Vector2Int(53, 14));
            shortRangeTurrets.Add(new Vector2Int(54, 14));
            shortRangeTurrets.Add(new Vector2Int(55, 14));

            foreach (var entity in entities)
            {
                entity.e.UpdateOffset = RandomUtils.RandomNumber(0, entity.e.UpdateRate);

                if(shortRangeTurrets.Contains(entity.location))
                {
                    entity.e.Variables[0] = new IntegerVariable(4);
                }
            }
        }

    }

}
