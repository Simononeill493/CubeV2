using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CubeV2
{
    public class SpriteAppearance : Appearance
    {
        public Func<string> GetSprite;

        public override Vector2 Size => Vector2.Zero;
        public float Scale = 1;

        public bool FlipHorizontal = false;

        public SpriteAppearance(float layer, Func<string> getSprite) : base(layer)
        {
            GetSprite = getSprite;
        }

        public SpriteAppearance(float layer, string sprite) : this(layer, () => { return sprite; }) { }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            DrawUtils.DrawSprite(spriteBatch, GetSprite(), position, Scale, 0, Vector2.Zero, Layer, SpriteEffects.None | (FlipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
        }
    }

}
