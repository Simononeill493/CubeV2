using CubeV2.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class UIGameGrid : UIGrid
    {
        public static Vector2 BoardPosition;

        private BoardAnimator _boardAnimator;

        public UIGameGrid(string id, int maxWidth, int maxHeight, TileAppearanceFactory appearanceFactory) : base(id, maxWidth, maxHeight, appearanceFactory) 
        {
            TileLeftClicked += (i, index) => GameInterface.TryLeftClickTile(index);
            TileRightClicked += (i, index) => GameInterface.TryRightClickTile(index);

            TileLeftPressed += (i, index) => GameInterface.TryLeftPressTile(index);
            TileRightPressed += (i, index) => GameInterface.TryRightPressTile(index);

            _boardAnimator = new BoardAnimator(DrawUtils.GameLayer3);
            AddAppearance(_boardAnimator);

            OnArrange += SendBoardSizesToAnimator;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 parentOffset, GameTime gameTime)
        {
            base.Draw(spriteBatch, parentOffset - GameCamera.TileSizeFloat, gameTime);
        }

        public override UIElement GenerateTile(string id)
        {
            return new UIGameTile(id);
        }

        private void SendBoardSizesToAnimator()
        {
            _boardAnimator.Arrange(CurrentSize, _indexWidthCurrent, _indexHeightCurrent, _tileWidth, _tileHeight, _padding);
        }
    }

    public class UIGameTile : UIElement
    {
        public UIGameTile(string id) : base(id) {}

        public override void CheckMouseOver(Vector2 mousePos)
        {
            if (Enabled)
            {
                MouseOver = UserInput.IsMouseInArea(_position-GameCamera.SubTileOffset, Appearance.Size, mousePos);
                return;
            }

            MouseOver = false;
        }
    }
}
