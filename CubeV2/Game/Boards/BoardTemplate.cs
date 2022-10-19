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

            foreach (var entity in Entities)
            {
                board.AddToBoard(entity.Value.GenerateEntity(), entity.Key);
            }

            return board;
        }
    }
}
