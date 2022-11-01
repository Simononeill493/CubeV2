
using Microsoft.Xna.Framework;

namespace CubeV2
{
    public class InstructionSelectionTileAppearanceFactory : TileAppearanceFactory
    {
        public InstructionSelectionTileAppearanceFactory(float bg,float fg) : base(bg,fg) {}

        public override TileAppearance CreateForeground(int index)
        {
            return new InstructionSelectionTileAppearance(index,_foregroundLayer);
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            var backgroundAppearance = new RectangleAppearance(width, height, Config.InstructionSelectorTileColor, _backgroundLayer);
            backgroundAppearance.OverrideColor(() => (GameInterface.IsFocusedOnInstructionOption(index) ? Color.Gray : Color.White));


            return backgroundAppearance;
        }

    }

}
