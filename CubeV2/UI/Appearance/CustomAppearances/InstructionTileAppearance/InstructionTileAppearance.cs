﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
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
        public InstructionTileAppearance(int index, float layer) : base(index, layer) { }

        public override Vector2 Size => Config.InstructionTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            var instruction = GameInterface.GetInstructionFromCurrentFocus(Index);
            if (instruction != null)
            {
                DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, instruction.Name, position, Color.White, 1, Layer);
            }
        }
    }
}
