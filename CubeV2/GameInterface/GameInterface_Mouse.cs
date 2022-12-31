using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        private static void _processMouseActions(UserInput input)
        {
            if(input.ScrollDifference!=0)
            {
                ScrollWheelTurned(input.ScrollDirection);
            }
        }

        private static int EntitySelection = 0;

        public static void ScrollWheelTurned(int scrollDirection)
        {
            EntitySelection += scrollDirection;
            if(EntitySelection<0)
            {
                EntitySelection = 0;
            }
        }

        public static void CurrentLeftClickAction(Tile tile, Vector2Int location,int index)
        {
            FocusTile(index);
            return;
            /*var distance = location.EuclideanDistance(_game.FocusEntity.Location);

            if (tile.Contents == null && distance <= Config.PlayerOperationalRadius)
            {
                var entities = EntityDatabase.GetAll();
                var template = entities[EntitySelection % entities.Count()];
                var newEntity = template.GenerateEntity();

                _game.CurrentBoard.AddEntityToBoard(newEntity, location);
            }*/
        }

        public static void CurrentRightClickAction(Tile tile, Vector2Int location,int index)
        {
            var distance = location.EuclideanDistance(_game.FocusEntity.Location);

            if (tile.Contents != null && distance <= Config.PlayerOperationalRadius)
            {
                _game.CurrentBoard.RemoveEntityFromBoard(tile.Contents);
            }
        }


        public static void LeftClickBoard(int index)
        {
            (var tile, var location) = _getBoardDetailsFromIndex(index);
            if (tile != null)
            {
                CurrentLeftClickAction(tile, location,index);
            }

        }
        public static void RightClickBoard(int index)
        {
            (var tile, var location) = _getBoardDetailsFromIndex(index);
            if(tile != null)
            {
                CurrentRightClickAction(tile, location,index);
            }
        }

        private static (Tile tile, Vector2Int Location) _getBoardDetailsFromIndex(int index)
        {
            var location = BoardUtils.IndexToXY(index, _game.CurrentBoard._width);
            var tile = _game.CurrentBoard.TryGetTile(location);

            if (tile != null)
            {
                return (tile, location);
            }

            return (null, Vector2Int.MinusOne);
        }


    }
}
