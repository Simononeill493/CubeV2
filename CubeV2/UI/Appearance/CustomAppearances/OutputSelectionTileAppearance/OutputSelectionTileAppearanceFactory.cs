
namespace CubeV2
{
    internal class OutputSelectionTileAppearanceFactory : TileAppearanceFactory
    {
        private int _spriteScale;

        public OutputSelectionTileAppearanceFactory(int scale, float bg,float fg) : base(bg,fg)
        {
            _spriteScale = scale;
        }

        public override TileAppearance CreateForeground(int index)
        {
            return new OutputSelectionTileAppearance(index, _spriteScale, _foregroundLayer);
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            return new RectangleAppearance(width, height, Config.InstructionSelectorTileColor, _backgroundLayer);
        }

    }

}
