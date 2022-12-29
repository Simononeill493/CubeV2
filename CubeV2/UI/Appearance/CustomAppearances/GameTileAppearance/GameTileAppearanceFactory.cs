
namespace CubeV2
{
    public class GameTileAppearanceFactory : TileAppearanceFactory
    {
        private float _spriteLayer;

        public GameTileAppearanceFactory(float groundLayer,float spriteLayer) : base(DrawUtils.BackgroundLayer,groundLayer)
        {
            _spriteLayer = spriteLayer;
        }

        public override TileAppearance CreateForeground(int index)
        {
            var appearance =  new GameTileAppearance(index,_foregroundLayer, _spriteLayer);
            return appearance;
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            return Appearance.NoAppearance;
        }

    }
}
