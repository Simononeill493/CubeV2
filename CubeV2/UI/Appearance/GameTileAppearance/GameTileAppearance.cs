using CubeV2.Utils;
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
        public override TileAppearance Create(int index)
        {
            return new GameTileAppearance(index);
        }
    }

    public class GameTileAppearance : TileAppearance
    {
        public GameTileAppearance(int gridIndex) : base(gridIndex){}

        public override Vector2 Size => Vector2.Zero;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite, position, Config.TileScale, DrawUtils.UILayer2);

            var contents = GameInterface.GetTile(this.Index).Contents;
            if (contents!=null)
            {
                DrawUtils.DrawSprite(spriteBatch, contents.Sprite, position, Config.TileScale, DrawUtils.UILayer3);
            }
        }
    }
}
