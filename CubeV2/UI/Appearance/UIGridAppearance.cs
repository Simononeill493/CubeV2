using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class UIGridAppearance : Appearance
    {
        public override Vector2 Size => _currentSize;

        private Vector2 _currentSize = Vector2.Zero;
        public void SetSizeFromGridItems(Vector2 newSize)
        {
            _currentSize = newSize;
        }

        public UIGridAppearance() : base(0)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            return;
        }
    }
}
