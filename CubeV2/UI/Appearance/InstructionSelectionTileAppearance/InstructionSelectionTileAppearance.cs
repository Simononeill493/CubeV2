using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        static Vector2 _textOffset = new Vector2(20, 30);

        public InstructionSelectionTileAppearance(int index,float layer) : base(index,layer) 
        {
            _layer = layer;
        }

        public override Vector2 Size => Config.InstructionOptionTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var instructionOption = GameInterface.GetInstructionOption(Index);
            if (instructionOption!=null)
            {
                DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, instructionOption.Name, position + _textOffset, Color.Black, 1, _layer);
            }
        }
    }

    public class InstructionSelectionTileAppearanceFactory : TileAppearanceFactory
    {
        public InstructionSelectionTileAppearanceFactory(float layer) : base(layer) {}

        public override TileAppearance Create(int index)
        {
            return new InstructionSelectionTileAppearance(index,_layer);
        }
    }

}
