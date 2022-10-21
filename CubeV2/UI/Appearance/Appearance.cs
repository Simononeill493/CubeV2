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
        public static Appearance NoAppearance = new NoAppearance();

        public abstract Vector2 Size { get; }

        public Color Color
        {
            get
            {
                if(_colorOverridden)
                {
                    return _getColor();
                }
                return _defaultColor;
            }
            set
            {
                _colorOverridden = false;
                _defaultColor = value;
            }
        }
        public void OverrideColor(Func<Color> getColor)
        {
            _colorOverridden = true;
            _getColor = getColor;
        }
        private bool _colorOverridden = false;
        private Func<Color> _getColor;
        private Color _defaultColor;
        

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}
