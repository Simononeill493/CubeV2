using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public class OutputControlTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;
        private int _instructionIndex;

        public OutputControlTileAppearance(int gridIndex, int instructionIndex, int scale,float layer) : base(gridIndex,layer)
        {
            _scale = scale;
            _instructionIndex = instructionIndex;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if(GameInterface.ControlOutputExists(_instructionIndex, Index))
            {
                var instruction = GameInterface.GetInstructionFromCurrentFocus(_instructionIndex);
                var targetIndex = instruction.ControlOutputs[Index];

                if(targetIndex >= 0)
                {
                    DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, targetIndex.ToString(), position, Color.Cyan, _scale, _layer);
                }
            }
        }
    }
}
