using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class GameTileAppearanceFactory : TileAppearanceFactory
    {
        private float _spriteLayer;

        public GameTileAppearanceFactory(float groundLayer,float spriteLayer) : base(groundLayer)
        {
            _spriteLayer = spriteLayer;
        }

        public override TileAppearance Create(int index)
        {
            return new GameTileAppearance(index,_layer, _spriteLayer);
        }
    }

    public class GameTileAppearance : TileAppearance
    {
        public GameTileAppearance(int gridIndex,float groundLayer,float spriteLayer) : base(gridIndex, groundLayer) {}

        public override Vector2 Size => Vector2.Zero;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite, position, Config.TileScale, 0, Vector2.Zero, DrawUtils.UILayer2);

            var contents = GameInterface.GetTile(this.Index).Contents;
            if (contents!=null)
            {
                DrawUtils.DrawEntity(spriteBatch, contents, position, Config.TileScale, DrawUtils.UILayer3);
            }
        }
    }
}
