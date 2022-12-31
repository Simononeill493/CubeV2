using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Color = Microsoft.Xna.Framework.Color;

namespace CubeV2
{
    public class TextAppearance : Appearance
    {
        public Func<string> GetText;

        public override Vector2 Size => Vector2.Zero;
        public float Scale = 1;

        public TextAppearance(Color color, float layer) : base(layer)
        {
            Color = color;
        }

        public TextAppearance(Color color, float layer, Func<string> getText) : this(color, layer)
        {
            GetText = getText;
        }

        public TextAppearance(Color color, float layer, string text) : this(color, layer, () => { return text; }) { }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, GetText(), position, Color, Scale, Layer);
        }
    }

}
