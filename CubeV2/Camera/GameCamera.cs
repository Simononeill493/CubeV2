using CubeV2.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace CubeV2.Camera
{
    public partial class GameCamera
    {
        public static int Scale { get; private set; }
        public static Vector2Int TileSizeInt { get; private set; }
        public static Vector2 TileSizeFloat { get; private set; }

        public static Vector2Int PixelOffset { get; private set; } = new Vector2Int(0, 0);
        public static Vector2Int IndexOffset { get; private set; } = new Vector2Int(0, 0);
        public static Vector2Int SubTileOffset { get; private set; } = new Vector2Int(0, 0);


        public static Vector2Int CameraGridSize = new Vector2Int(0, 0);

        public static void SetScale(int scale)
        {
            if(scale < Config.MinimumCameraScale | scale > Config.MaximumCameraScale)
            {
                return;
            }

            var potentialTileSize = _getTileSizeForScale(scale);
            if(!_isTileSizeValid(potentialTileSize))
            {
                return;
            }

            Scale = scale;
            TileSizeFloat = Scale * Config.TileBaseSizeFloat;
            TileSizeInt = new Vector2Int(TileSizeFloat);
            CameraGridSize = (Config.GameUIGridMaxSize / potentialTileSize).Ceiled() + Vector2Int.Two;

            AllUIElements.GetGameGrid().Arrange(CameraGridSize, TileSizeInt, Config.GameUIGridPadding);
        }

        public static void SetPixelOffset(Vector2 pixelOffset)
        {
            //Console.WriteLine("PixelOffset change: " + (pixelOffset-PixelOffset).ToStringRounded(10));

            PixelOffset = pixelOffset.Rounded();

            IndexOffset = PixelOffset / TileSizeInt;
            SubTileOffset = (PixelOffset - (IndexOffset * Config.TileBaseSizeInt * Scale)) % TileSizeInt;

            //Console.WriteLine("\n\nPixelOffset " + PixelOffset.ToStringRounded(3) + "\n" + IndexOffset + "\n" + SubTileOffset.ToStringRounded(3));
        }


        public static void CenterCameraOnPlayer()
        {
            var playerIndexOffset = GameInterface._game.FocusEntity.Location - CameraGridSize / 2;
            var playerPixelOffset = playerIndexOffset * TileSizeFloat;

            SetPixelOffset(playerPixelOffset);
        }

        public static bool IsPlayerFullyInCamera()
        {
            //TODO unfinished and broken 
            var rect = new Rectangle(IndexOffset.X, IndexOffset.Y, CameraGridSize.X, CameraGridSize.Y);
            var ans = rect.Contains(GameInterface._game.FocusEntity.Location.ToVector2());

            Console.WriteLine(rect + "\t" + GameInterface._game.FocusEntity.Location + "\t" + ans);
            return ans;
        }

        public static (Vector2Int boardLocation, int realIndex) GetBoardLocationFromCameraIndex(int index)
        {
            var realLocation = BoardUtils.IndexToXY(index, CameraGridSize.X) + IndexOffset;
            var realIndex = BoardUtils.XYToIndex(realLocation, GameInterface._game.CurrentBoard._width);

            return (realLocation, realIndex);
            //return (Vector2Int.MinusOne, -1);
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


        private static bool _isTileSizeValid(Vector2 potentialTileSize)
        {
            if (potentialTileSize.X > Config.GameUIGridMaxSize.X || potentialTileSize.Y > Config.GameUIGridMaxSize.Y || potentialTileSize.X < 1 || potentialTileSize.Y < 1)
            {
                return false;
            }

            return true;
        }

        private static Vector2 _getTileSizeForScale(int scale)
        {
            var potentialTileSize = scale * (Config.TileBaseSizeFloat + new Vector2(Config.GameUIGridPadding, Config.GameUIGridPadding));
            return potentialTileSize;
        }
    }
}

