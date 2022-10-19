using CubeV2;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace CubeV2
{
    public class UIGrid : UIElement
    {
        private UIGrid(string id) : base(id) { }
        private static string _generateTileID(string gridId, int x, int y) => gridId + '_' + x + '_' + y;
        private static Vector2 _generateTileOffset(int x, int y, int width, int height, int padding) => new Vector2(((width + padding) * x) + padding, ((height + padding) * y) + padding);

        public static UIGrid Make(string id, int gridWidth, int gridHeight, int tileWidth, int tileHeight, int internalPadding, Color color, float layer, TileAppearanceFactory appearanceFactory)
        {
            var grid = new UIGrid(id);
            grid.Appearance = new NoAppearance();

            int index = 0;

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    var tileId = _generateTileID(id, x, y);
                    var tileContentsAppearance = appearanceFactory.Create(index);

                    var tile = _makeTile(tileId, tileWidth, tileHeight, color, layer, tileContentsAppearance);
                    tile.Offset = _generateTileOffset(x, y, tileWidth, tileHeight, internalPadding);

                    int indexCaptured = index;
                    tile.OnLeftClick += (input) => { grid.TileLeftClicked?.Invoke(input, indexCaptured); };
                    tile.OnRightClick += (input) => { grid.TileRightClicked?.Invoke(input, indexCaptured); };

                    grid.AddChildren(tile);
                    index++;
                }
            }

            return grid;
        }
        private static UIElement _makeTile(string id, int width, int height, Color color, float layer, TileAppearance tileAppearance)
        {
            var tile = new UIElement(id);

            var tileBackground = new RectangleAppearance(width, height, color, layer);
            tile.Appearance = MultiAppearance.Create(tileBackground, tileAppearance);
            tile.Clickable = true;

            return tile;
        }

        public event Action<UserInput, int> TileLeftClicked;
        public event Action<UserInput, int> TileRightClicked;

    }

    public abstract class TileAppearanceFactory
    {
        public abstract TileAppearance Create(int index);
    }

    public abstract class TileAppearance : Appearance
    {
        public TileAppearance(int index)
        {
            Index = index;
        }

        public int Index;
    }
}
