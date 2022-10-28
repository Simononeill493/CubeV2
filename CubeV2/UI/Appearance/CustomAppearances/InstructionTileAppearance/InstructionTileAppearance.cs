using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class InstructionTileAppearance : TileAppearance
    {
        public InstructionTileAppearance(int index,float layer) : base(index,layer){}

        public override Vector2 Size => Config.InstructionTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var instruction = GameInterface.GetInstructionFromCurrentFocus(Index);
            if (instruction!=null)
            {
                DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, instruction.Name, position, Color.White, 1, Layer);
            }
        }
    }
}
