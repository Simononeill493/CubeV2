using CubeV2.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;

namespace CubeV2
{
    public class TextAppearance : Appearance
    {
        public Func<string> GetText;

        public override Vector2 Size => Vector2.Zero;
        public float Layer;

        public TextAppearance(Color color, float layer)
        {
            Color = color;
            Layer = layer;
        }

        public TextAppearance(Color color, float layer,string text) : this(color,layer)
        {
            GetText = () => { return text; };
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(DrawUtils.PressStart2PFont, GetText(), position, Color, 0, Vector2.Zero, 1, SpriteEffects.None, Layer);
        }
    }
}
