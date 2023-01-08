using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public class GameTileAppearance : TileAppearance
    {
        private float _spriteLayer;
        public GameTileAppearance(int gridIndex,float groundLayer,float spriteLayer) : base(gridIndex, groundLayer) 
        {
            _spriteLayer = spriteLayer;
        }

        public override Vector2 Size => Config.TileBaseSize * Config.TileScale;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite, position, Config.TileScale, 0, Vector2.Zero, Layer);

            var contents = GameInterface._game.CurrentBoard.TryGetTile(this.Index).Contents;
            if (contents!=null)
            {
                DrawUtils.DrawEntity(spriteBatch, contents, position, Config.TileScale, _spriteLayer);
            }
        }
    }
}
