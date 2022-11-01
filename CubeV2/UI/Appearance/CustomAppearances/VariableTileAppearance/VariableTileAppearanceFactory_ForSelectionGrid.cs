
namespace CubeV2
{
    public class VariableTileAppearanceFactory_ForSelectionGrid : TileAppearanceFactory
    {
        private int _spriteScale;

        public VariableTileAppearanceFactory_ForSelectionGrid(int scale,float bg,float fg) : base(bg,fg)
        {
            _spriteScale = scale;
        }

        public override TileAppearance CreateForeground(int index)
        {
            return new VariableTileAppearance_Grid(index, _spriteScale,_foregroundLayer);
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            var backgroundAppearance = new RectangleAppearance(width, height, Config.SelectionTileVariableColor, _backgroundLayer);
            backgroundAppearance.OverrideColor(() => (GameInterface.IsFocusedOnVariableOption(index) ? Config.InstructionTileAssignedVariableHighlightColor : Config.SelectionTileVariableColor));

            return backgroundAppearance;
        }

    }

}
