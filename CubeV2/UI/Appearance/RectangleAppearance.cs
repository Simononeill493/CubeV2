using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class RectangleAppearance : Appearance
    {
        public override Vector2 Size => _size;
        private Vector2 _size;

        public RectangleAppearance(int width, int height, Microsoft.Xna.Framework.Color color, float layer) : this(new Vector2(width, height), color, layer) { }

        public RectangleAppearance(Vector2 size, Microsoft.Xna.Framework.Color color, float layer) :base(layer)
        {
            _size = size;
            Color = color;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawRect(spriteBatch, position, Size, Color, Layer);
        }
    }
}
