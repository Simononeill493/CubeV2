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
        public static Appearance NoAppearance => new NoAppearance();

        public abstract Vector2 Size { get; }
        public float Layer { get; }

        public Appearance(float layer)
        {
            Layer = layer;
        }

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
        

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime);

        /*public void DrawOverlay(SpriteBatch spriteBatch, Vector2 position,Color color,float layer)
        {
            DrawUtils.DrawRect(spriteBatch,position,Size,color,layer);
        }*/
    }
}
