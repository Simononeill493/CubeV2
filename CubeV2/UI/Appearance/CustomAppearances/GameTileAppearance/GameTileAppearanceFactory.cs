
using SAME;

namespace CubeV2
{
    public class GameTileAppearanceFactory : TileAppearanceFactory
    {
        private float _spriteLayer;
        private float _spriteMeterLayer;
        private float _spriteMeterLayer2;

        public GameTileAppearanceFactory(float groundLayer, float spriteLayer, float spriteMeterLayer, float spriteMeterLayer2) : base(CubeDrawUtils.BackgroundLayer, groundLayer)
        {
            _spriteLayer = spriteLayer;
            _spriteMeterLayer = spriteMeterLayer;
            _spriteMeterLayer2 = spriteMeterLayer2;
        }

        public override TileAppearance CreateForeground(int index)
        {
            var appearance = new GameTileAppearance(index, _foregroundLayer, _spriteLayer, _spriteMeterLayer, _spriteMeterLayer2);
            return appearance;
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            return Appearance.NoAppearance;
        }

    }
}
