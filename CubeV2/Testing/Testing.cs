using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class Testing
    {
        public static void Go()
        {
            var captureRender = GameWrapper.DoDraw;
            GameWrapper.DoDraw = false;

            var testBoard = new Board(3, 3);
            var testEntity = new Entity("Test", "Test",null);
            testEntity.MaxEnergy = 100;
            testEntity.SetEnergyToMax();

            testBoard.AddEntityToBoard(testEntity, new Vector2Int(0, 0));

            if (testBoard.TryGetTile(Vector2Int.Zero).Contents!=testEntity)
            {
                throw new Exception("Test failed.");
            }

            if (testEntity.Location != Vector2Int.Zero)
            {
                throw new Exception("Test failed.");
            }

            testEntity.Instructions[0][0] =  new MoveInstruction(RelativeDirection.BackwardRight);

            GameInterface.InitializeBoardlessGame();
            GameInterface.ManualSetNewBoard(testBoard);
            testBoard.Tick(null);

            if (testBoard.TryGetTile(Vector2Int.Zero).Contents != null)
            {
                throw new Exception("Test failed.");
            }

            if (testBoard.TryGetTile(Vector2Int.One).Contents != testEntity)
            {
                throw new Exception("Test failed.");
            }

            if (testEntity.Location != Vector2Int.One)
            {
                throw new Exception("Test failed.");
            }


            testBoard.RemoveEntityFromCurrentTile(testEntity);

            if (testBoard.TryGetTile(Vector2Int.One).Contents != null)
            {
                throw new Exception("Test failed.");
            }

            if (testEntity.Location != Vector2Int.MinusOne)
            {
                throw new Exception("Test failed.");
            }

            GameInterface.ManualUnsetBoard();
            GameWrapper.DoDraw = captureRender;
        }
    }
}
