using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public class OutputTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;
        private int _instructionIndex;

        public OutputTileAppearance(int gridIndex, int instructionIndex, int scale,float layer) : base(gridIndex,layer)
        {
            _scale = scale;
            _instructionIndex = instructionIndex;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if(GameInterface.OutputExists(_instructionIndex, Index))
            {
                var instruction = GameInterface.GetInstructionFromCurrentFocus(_instructionIndex);
                var outputNum = instruction.OutputTargetVariables[Index];

                if(outputNum>=0)
                {
                    DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, outputNum.ToString(), position, Color.Magenta, _scale, Layer);
                }
            }
        }
    }
}
