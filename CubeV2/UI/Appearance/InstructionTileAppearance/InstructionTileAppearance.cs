using CubeV2.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class InstructionTileAppearance : TileAppearance
    {
        public InstructionTileAppearance(int index) : base(index){}

        public override Vector2 Size => Config.InstructionTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var name = "";
            if (GameInterface.InstructionExists(Index))
            {
                name = GameInterface.FocusedInstructions[Index].Name;
            }

            spriteBatch.DrawString(DrawUtils.PressStart2PFont, name, position, Color.White);
        }
    }
}
