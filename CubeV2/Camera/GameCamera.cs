using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2.Camera
{
    public partial class GameCamera
    {
        public static int Scale;

        public static Vector2Int IndexOffset = new Vector2Int(0, 0);
        public static Vector2Int PixelOffsetFromGrid = new Vector2Int(0, 0);

        public static void CenterCameraOnPlayer()
        {
            SetCameraOffset(GameInterface._game.FocusEntity.Location - CameraGridSize / 2);
        }

        public static bool IsPlayerInCamera()
        {
            return new Rectangle(IndexOffset.X, IndexOffset.Y, CameraGridSize.X, CameraGridSize.Y).Contains(GameInterface._game.FocusEntity.Location.ToVector2());
            //return true;
        }


        public static (Vector2Int boardLocation, int realIndex) GetBoardLocationFromCameraIndex(int index)
        {
            var realLocation = BoardUtils.IndexToXY(index, CameraGridSize.X) + IndexOffset;
            var realIndex = BoardUtils.XYToIndex(realLocation, GameInterface._game.CurrentBoard._width);

            return (realLocation, realIndex);
            //return (Vector2Int.MinusOne, -1);
        }


        public static Vector2Int TileSizeInt { get; private set; }
        public static Vector2 TileSizeFloat { get; private set; }


        public static Vector2Int CameraGridSize = new Vector2Int(0, 0);

        public static (Vector2Int boardLocation, int realIndex) GetGameTileFromCameraIndex(int index)
        {
            var realLocation = BoardUtils.IndexToXY(index, CameraGridSize.X) + IndexOffset;
            var realIndex = BoardUtils.XYToIndex(realLocation, GameInterface._game.CurrentBoard._width);

            return (realLocation, realIndex);
        }


        public static void SetCameraConfig(int scale)
        {
            var potentialTileSize = scale * (Config.TileBaseSize + new Vector2(Config.GameUIGridPadding, Config.GameUIGridPadding));
            if (potentialTileSize.X > Config.GameUIGridMaxSize.X || potentialTileSize.X < 1 || potentialTileSize.Y > Config.GameUIGridMaxSize.Y || potentialTileSize.Y < 1)
            {
                return;
            }

            var maxSizeAtThisScale = Config.GameUIGridMaxSize / potentialTileSize;
            SetCameraConfig(scale, new Vector2Int(maxSizeAtThisScale));
        }

        public static void SetCameraConfig(int scale, Vector2Int size)
        {
            if (scale < 1)
            {
                return;
            }

            Scale = scale;
            TileSizeFloat = Scale * Config.TileBaseSize;
            TileSizeInt = new Vector2Int(TileSizeFloat);
            CameraGridSize = size;

            var gameGrid = (UIGrid)AllUIElements.GetUIElement(Config.GameGridName);
            gameGrid.Arrange(CameraGridSize, TileSizeInt, Config.GameUIGridPadding);
        }

        public static void SetCameraOffset(Vector2Int offset)
        {
            if (Config.AllowCameraMovement)
            {
                IndexOffset = offset;
            }
        }


        public static void RevealMapToPlayer()
        {
            throw new NotImplementedException();
            /*var location = _game.FocusEntity.Location;
            _game.CurrentBoard.TryGetTile(location).Seen = true;

            for(int x= location.X - Config.PlayerVisualRadius; x< location.X + Config.PlayerVisualRadius;x++)
            {
                for (int y = location.Y - Config.PlayerVisualRadius; y < location.Y + Config.PlayerVisualRadius; y++)
                {
                    var locInView = new Vector2Int(x, y);
                    var tile = _game.CurrentBoard.TryGetTile(locInView);
                    if(tile!=null && !tile.Seen)
                    {
                        foreach(var adjacent in locInView.GetAdjacentPoints())
                        {
                            var adjTile = _game.CurrentBoard.TryGetTile(adjacent);
                            if(adjTile != null && adjTile.Seen && ((adjTile.Contents == null) || adjTile.Contents.HasTag(Config.PlayerTag)))
                            {
                                tile.Seen = true;
                                goto Done;
                            }
                        }
                    }

                    Done: continue;
                }

            }*/
    }
}
}
