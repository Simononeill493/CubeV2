namespace CubeV2
{
    public abstract class TileAppearanceFactory
    {
        protected float _backgroundLayer;
        protected float _foregroundLayer;

        public TileAppearanceFactory(float backgroundLayer, float foregroundLayer)
        {
            _backgroundLayer = backgroundLayer;
            _foregroundLayer = foregroundLayer;
        }

        public abstract TileAppearance CreateForeground(int index);
        public abstract Appearance CreateBackground(int index, int width, int height);
    }
}
