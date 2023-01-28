using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        public static int CameraScale = 2;
        public static Vector2Int CameraTileSize { get; private set; }
        public static Vector2 _cameraTileSizeFloat { get; private set; }

        public static Vector2Int CameraOffset = new Vector2Int(0, 0);
        public static Vector2Int CameraSize = new Vector2Int(Config.GameUIGridDefaultWidth/2, Config.GameUIGridDefaultHeight/2);

        public static (Vector2Int realLocation,int realIndex) UITileGetRealTile(int index)
        {
            var realLocation = BoardUtils.IndexToXY(index, CameraSize.X) + CameraOffset;
            var realIndex = BoardUtils.XYToIndex(realLocation, _game.CurrentBoard._width);

            return (realLocation, realIndex);
        }

        public static void SetCameraConfig(int scale)
        {
            var potentialTileSize = scale * (Config.TileBaseSize + new Vector2(Config.GameUIGridPadding,Config.GameUIGridPadding));
            if(potentialTileSize.X > Config.GameUIGridMaxSize.X || potentialTileSize.X<1 || potentialTileSize.Y > Config.GameUIGridMaxSize.Y || potentialTileSize.Y < 1)
            {
                return;
            }

            var maxSizeAtThisScale = Config.GameUIGridMaxSize / potentialTileSize;
            SetCameraConfig(scale, new Vector2Int(maxSizeAtThisScale));
        }

        public static void SetCameraConfig(int scale,Vector2Int size)
        {
            if(scale<1)
            {
                return;
            }    

            CameraScale = scale;
            _cameraTileSizeFloat = CameraScale * Config.TileBaseSize;
            CameraTileSize = new Vector2Int(_cameraTileSizeFloat);
            CameraSize = size;

            var gameGrid = (UIGrid)AllUIElements.GetUIElement(Config.GameGridName);
            gameGrid.Arrange(CameraSize, CameraTileSize, Config.GameUIGridPadding);
        }
    }
}
