using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CubeV2
{
    public class GifAppearance : Appearance
    {
        public string Gif;

        public override Vector2 Size => Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public float Transparency = 1;

        public GifAppearance(float layer, string gif) : base(layer)
        {
            Gif = gif;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawGif(spriteBatch, Gif, position, Scale, 0, Vector2.Zero, Layer,Color.White * Transparency);
        }
    }

}
