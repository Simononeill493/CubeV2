using CubeV2.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2.UI.Appearance.InstructionSelectionTileAppearance
{
    internal class InstructionSelectionTileAppearance : TileAppearance
    {
        static Vector2 _textOffset = new Vector2(20, 30);

        public InstructionSelectionTileAppearance(int index,float layer) : base(index,layer) 
        {
            _layer = layer;
        }

        public override Vector2 Size => Config.InstructionSelectorTileSize;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var name = "";
            if (GameInterface.SelectableInstructionExists(Index))
            {
                name = GameInterface.SelectableInstructions[Index].Name;
            }

            DrawUtils.DrawText(spriteBatch, DrawUtils.PressStart2PFont, name, position + _textOffset, Color.Black, _layer);
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
