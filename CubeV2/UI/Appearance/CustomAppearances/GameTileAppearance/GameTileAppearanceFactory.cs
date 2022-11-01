
namespace CubeV2
{
    public class GameTileAppearanceFactory : TileAppearanceFactory
    {
        private float _spriteLayer;

        public GameTileAppearanceFactory(float groundLayer,float spriteLayer) : base(groundLayer,spriteLayer)
        {
            _spriteLayer = spriteLayer;
        }

        public override TileAppearance CreateForeground(int index)
        {
            return new GameTileAppearance(index,_foregroundLayer, _spriteLayer);
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            return Appearance.NoAppearance;
        }

    }
}
