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
        private static Vector2 _generateTileOffset(int x, int y, int width, int height, int padding) => new Vector2((width + padding) * x + padding, (height + padding) * y + padding);

        public static UIGrid Make(string id, Vector2Int gridSize, Vector2Int tileSize, int internalPadding, TileAppearanceFactory appearanceFactory)
         => Make(id, gridSize.X, gridSize.Y, tileSize.X, tileSize.Y, internalPadding, appearanceFactory);

        public static UIGrid Make(string id, int gridWidth, int gridHeight, int tileWidth, int tileHeight, int internalPadding, TileAppearanceFactory appearanceFactory)
        {
            var grid = new UIGrid(id);

            int index = 0;

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    var tile = new UIElement(_generateTileID(id, x, y));

                    var backgroundAppearance = appearanceFactory.CreateBackground(index, tileWidth, tileHeight);
                    var foregroundAppearance = appearanceFactory.CreateForeground(index);

                    tile.AddAppearances(backgroundAppearance, foregroundAppearance);
                    tile.SetOffset(_generateTileOffset(x, y, tileWidth, tileHeight, internalPadding));

                    int indexCaptured = index;
                    tile.AddLeftClickAction((input) => { grid.TileLeftClicked?.Invoke(input, indexCaptured); });
                    tile.AddRightClickAction((input) => { grid.TileRightClicked?.Invoke(input, indexCaptured); });

                    grid.AddChildren(tile);
                    index++;
                }
            }

            var size = new Vector2(((tileWidth + internalPadding) * gridWidth), (tileHeight + internalPadding) * gridHeight);
            grid.SetManualSize(size);

            return grid;
        }

        public event Action<UserInput, int> TileLeftClicked;
        public event Action<UserInput, int> TileRightClicked;
    }
}
