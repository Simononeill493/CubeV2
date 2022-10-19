using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public abstract class Appearance
    {
        public abstract Vector2 Size { get; }

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}
