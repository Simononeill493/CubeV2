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
        public static Vector2 TileSizeFloat { get; private set; }
        public static Vector2Int TileSizeInt { get; private set; }
        private static Vector2Int _mapActualSizePixels = Vector2Int.Zero;

        public static Vector2Int PixelOffset { get; private set; } = new Vector2Int(0, 0);
        public static Vector2Int IndexOffset { get; private set; } = new Vector2Int(0, 0);
        public static Vector2Int SubTileOffset { get; private set; } = new Vector2Int(0, 0);

        private static Vector2 _mapTopLeftOffset = Vector2.Zero;
        private static Vector2 _mapBottomRightOffset = Vector2.Zero;

        public static Vector2Int CameraGridSize = Vector2Int.Zero;

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
            TileSizeFloat = Scale * Config.TileBaseSize;
            TileSizeInt = new Vector2Int(TileSizeFloat);
            CameraGridSize = (Config.GameBoardScreenSpaceAllocated / potentialTileSize).Ceiled() + Config.GameUIGridIndexPadding;

            _mapActualSizePixels = GameInterface._game.CurrentBoard.Size * TileSizeInt;
            _mapTopLeftOffset = -TileSizeFloat;
            _mapBottomRightOffset = (_mapActualSizePixels - (Config.GameBoardScreenSpaceAllocated + (Config.GameUIGridIndexPadding / 2 * TileSizeInt)));

            AllUIElements.GetGameGrid().Arrange(CameraGridSize, TileSizeInt, Config.GameUIGridPadding);
        }

        public static void SetPixelOffset(Vector2 pixelOffset)
        {
            PixelOffset = pixelOffset.Clamped(_mapTopLeftOffset, _mapBottomRightOffset).Rounded();
            IndexOffset = PixelOffset / TileSizeInt;
            SubTileOffset = (PixelOffset - (IndexOffset * TileSizeInt)) % TileSizeInt;




            //Console.WriteLine("PixelOffset " + PixelOffset + "\t" + (_mapActualSizePixels - (Config.GameBoardScreenSpaceAllocated + (Config.GameUIGridIndexPadding / 2 * TileSizeInt))));
            //Console.WriteLine("\n\nPixelOffset " + PixelOffset + "\n" + IndexOffset + "\n" + SubTileOffset);
            //Console.WriteLine("PixelOffset change: " + (pixelOffset-PixelOffset).ToStringRounded(10));
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

            //Console.WriteLine(rect + "\t" + GameInterface._game.FocusEntity.Location + "\t" + ans);
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
            if (potentialTileSize.X > Config.GameBoardScreenSpaceAllocated.X || potentialTileSize.Y > Config.GameBoardScreenSpaceAllocated.Y || potentialTileSize.X < 1 || potentialTileSize.Y < 1)
            {
                return false;
            }

            return true;
        }

        private static Vector2 _getTileSizeForScale(int scale)
        {
            var potentialTileSize = scale * (Config.TileBaseSize + new Vector2(Config.GameUIGridPadding, Config.GameUIGridPadding));
            return potentialTileSize;
        }
    }
}

