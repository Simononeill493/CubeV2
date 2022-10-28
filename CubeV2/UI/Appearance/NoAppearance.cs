using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class NoAppearance : Appearance
    {
        public override Vector2 Size => Vector2.Zero;

        public NoAppearance() :base(0)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            return;
        }
    }
}
