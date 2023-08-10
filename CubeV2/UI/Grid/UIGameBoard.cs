using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class UIGameGrid : UIGrid
    {
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

        private void SendBoardSizesToAnimator()
        {
            _boardAnimator.Arrange(CurrentSize, _indexWidthCurrent, _indexHeightCurrent, _tileWidth, _tileHeight, _padding);
        }
    }
}
