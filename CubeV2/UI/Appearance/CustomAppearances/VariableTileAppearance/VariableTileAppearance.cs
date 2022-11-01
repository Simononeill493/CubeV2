using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public abstract class VariableTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;

        public VariableTileAppearance(int gridIndex, int scale,float layer) : base(gridIndex,layer)
        {
            _scale = scale;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var variable = GetSource();
            if(variable!=null)
            {
                variable.Draw(spriteBatch, position, _scale, Layer);
            }
        }

        public abstract IVariable GetSource();

    }

}
