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
        //private Dictionary<(int,int),UIElement> _tiles;
        private List<UIElement> _tiles;
        private TileAppearanceFactory _appearanceFactory;

        private static string _generateTileID(string gridId, int x, int y) => gridId + '_' + x*y + " (" + x + ' ' + y + ')';
        private static Vector2 _generateTileOffset(int x, int y, int width, int height, int padding) => new Vector2((width + padding) * x + padding, (height + padding) * y + padding);

        public UIGrid(string id, Vector2Int maxSize, TileAppearanceFactory appearanceFactory): this(id, maxSize.X, maxSize.Y, appearanceFactory) { }
        public UIGrid(string id, int maxWidth, int maxHeight, TileAppearanceFactory appearanceFactory) : base(id)
        {
            _appearanceFactory = appearanceFactory;
            _tiles = new List<UIElement>();

            int index = 0;

            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    var tile = new UIElement(_generateTileID(id, x, y));

                    int indexCaptured = index;
                    tile.AddLeftClickAction((input) => { TileLeftClicked?.Invoke(input, indexCaptured); });
                    tile.AddRightClickAction((input) => { TileRightClicked?.Invoke(input, indexCaptured); });

                    AddChildren(tile);
                    _tiles.Add(tile);
                    index++;
                }
            }
        }

        public void Arrange(Vector2Int size,Vector2Int tileSize,int internalPadding) => Arrange(size.X, size.Y, tileSize.X, tileSize.Y, internalPadding);
        public void Arrange(int width,int height,int tileWidth,int tileHeight,int internalPadding)
        {
            int index = 0;

            _tiles.ForEach((t) => t._alwaysDisabled = true);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = _tiles[index];

                    var backgroundAppearance = _appearanceFactory.CreateBackground(index, tileWidth, tileHeight);
                    var foregroundAppearance = _appearanceFactory.CreateForeground(index);

                    tile.ClearApperances();
                    tile.AddAppearances(backgroundAppearance, foregroundAppearance);
                    tile.SetOffset(_generateTileOffset(x, y, tileWidth, tileHeight, internalPadding));

                    tile._alwaysDisabled = false;
                    index++;
                }
            }

            var size = new Vector2(((tileWidth + internalPadding) * width), (tileHeight + internalPadding) * height);
            SetManualSize(size);
        }

        public event Action<UserInput, int> TileLeftClicked;
        public event Action<UserInput, int> TileRightClicked;
    }
}
