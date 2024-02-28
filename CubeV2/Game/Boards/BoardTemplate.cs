using SAME;
using System.Collections.Generic;

namespace CubeV2
{
    public class BoardTemplate
    {
        public Dictionary<Vector2Int, EntityTemplate> EntitiesToPlace = new Dictionary<Vector2Int, EntityTemplate>();

        public int Width;
        public int Height;

        public Board GenerateBoard()
        {
            var board = new Board(Width, Height);

            var generatedEntities = new Dictionary<Vector2Int, Entity>();
            foreach (var entity in EntitiesToPlace)
            {
                generatedEntities[entity.Key] = entity.Value.GenerateEntity();
            }

            OnEntitiesGenerated(generatedEntities);

            foreach (var entity in generatedEntities)
            {
                board.TryAddEntityToBoard(entity.Value, entity.Key);
            }

            OnBoardGenerated(board);

            return board;
        }

        public virtual void OnEntitiesGenerated(Dictionary<Vector2Int, Entity> entitiesGenerated) { }
        public virtual void OnBoardGenerated(Board b) { }

    }

    public class FortressTutorialTemplate : BoardTemplate
    {
        public Dictionary<Vector2Int, string> GroundSprites = new Dictionary<Vector2Int, string>();

        public override void OnEntitiesGenerated(Dictionary<Vector2Int, Entity> generatedEntities)
        {
            var shortRangeTurrets = new List<Vector2Int>();
            shortRangeTurrets.Add(new Vector2Int(86, 20));
            shortRangeTurrets.Add(new Vector2Int(53, 14));
            shortRangeTurrets.Add(new Vector2Int(54, 14));
            shortRangeTurrets.Add(new Vector2Int(55, 14));

            foreach (var entity in generatedEntities)
            {
                entity.Value.UpdateOffset = RandomUtils.RandomNumber(0, entity.Value.UpdateRate);

                if (shortRangeTurrets.Contains(entity.Key))
                {
                    entity.Value.Variables[0] = new IntegerVariable(4);
                }
            }
        }

        public override void OnBoardGenerated(Board b)
        {
            foreach (var groundSprite in GroundSprites)
            {
                var tile = b.TryGetTile(groundSprite.Key);
                tile.Sprite = groundSprite.Value;
            }

            base.OnBoardGenerated(b);
        }

    }

}
