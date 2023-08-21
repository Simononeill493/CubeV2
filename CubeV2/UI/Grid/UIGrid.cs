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
        public UIGridAppearance GridAppearance;
        private TileAppearanceFactory _tileAppearanceFactory;

        protected Vector2 CurrentSize => GridAppearance.Size;
        protected int _indexWidthCurrent;
        protected int _indexHeightCurrent;
        protected int _tileWidth;
        protected int _tileHeight;
        protected int _padding;

        private List<UIElement> _tiles;
        protected int _indexWidthMax;
        protected int _indexHeightMax;

        private static string _generateTileID(string gridId, int x, int y) => gridId + '_' + x * y + " (" + x + ' ' + y + ')';
        private static Vector2 _generateTileOffset(int x, int y, int tileWidth, int tileHeight, int padding) => new Vector2((tileWidth + padding) * x + padding, (tileHeight + padding) * y + padding);

        public UIGrid(string id, Vector2Int maxSize, TileAppearanceFactory appearanceFactory) : this(id, maxSize.X, maxSize.Y, appearanceFactory) { }
        public UIGrid(string id, int maxIndexWidth, int maxIndexHeight, TileAppearanceFactory appearanceFactory) : base(id)
        {
            GridAppearance = new UIGridAppearance();
            Appearance = GridAppearance;

            _tileAppearanceFactory = appearanceFactory;
            _tiles = new List<UIElement>();

            int index = 0;

            for (int y = 0; y < maxIndexHeight; y++)
            {
                for (int x = 0; x < maxIndexWidth; x++)
                {
                    var tile = GenerateTile(_generateTileID(id, x, y));

                    int indexCaptured = index;
                    tile.AddLeftClickAction((input) => { TileLeftClicked?.Invoke(input, indexCaptured); });
                    tile.AddRightClickAction((input) => { TileRightClicked?.Invoke(input, indexCaptured); });

                    tile.AddLeftMousePressedAction((input) => { TileLeftPressed?.Invoke(input, indexCaptured); });
                    tile.AddRightMousePressedAction((input) => { TileRightPressed?.Invoke(input, indexCaptured); });

                    AddChildren(tile);
                    _tiles.Add(tile);
                    index++;
                }
            }

            _indexWidthMax = maxIndexWidth;
            _indexHeightMax = maxIndexHeight;
        }

        public virtual UIElement GenerateTile(string id)
        {
            return new UIElement(id);
        }

        public void Arrange(Vector2Int size, Vector2Int tileSize, int internalPadding) => Arrange(size.X, size.Y, tileSize.X, tileSize.Y, internalPadding);
        public void Arrange(int indexWidth, int indexHeight, int tileWidth, int tileHeight, int internalPadding)
        {
            int index = 0;

            _tiles.ForEach((t) => t._forceDisable = true);

            for (int y = 0; y < indexHeight; y++)
            {
                for (int x = 0; x < indexWidth; x++)
                {
                    var tile = _tiles[index];

                    var backgroundAppearance = _tileAppearanceFactory.CreateBackground(index, tileWidth, tileHeight);
                    var foregroundAppearance = _tileAppearanceFactory.CreateForeground(index);

                    tile.ClearApperances();
                    tile.AddAppearances(backgroundAppearance, foregroundAppearance);
                    tile.SetOffset(_generateTileOffset(x, y, tileWidth, tileHeight, internalPadding));

                    tile._forceDisable = false;
                    index++;
                }
            }

            var size = new Vector2(((tileWidth + internalPadding) * indexWidth), (tileHeight + internalPadding) * indexHeight);
            GridAppearance.SetSizeFromGridItems(size);

            _indexWidthCurrent = indexWidth;
            _indexHeightCurrent = indexHeight;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _padding = internalPadding;

            OnArrange?.Invoke();
        }

        public event Action OnArrange;

        public event Action<UserInput, int> TileLeftClicked;
        public event Action<UserInput, int> TileRightClicked;

        public event Action<UserInput, int> TileLeftPressed;
        public event Action<UserInput, int> TileRightPressed;
    }
}
