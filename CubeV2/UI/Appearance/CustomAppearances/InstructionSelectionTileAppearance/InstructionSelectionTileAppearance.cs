using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2
{
    internal class InstructionSelectionTileAppearance : TileAppearance
    {
        static Vector2 _textOffset = new Vector2(20, 15);

        public InstructionSelectionTileAppearance(int index, float layer) : base(index, layer) { }

        public override Vector2 Size => Config.InstructionOptionTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            var instructionOption = GameInterface.GetInstructionOption(Index);
            if (instructionOption != null)
            {
                DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, instructionOption.Name, position + _textOffset, Color.Black, 1, Layer);
            }
        }
    }

}
