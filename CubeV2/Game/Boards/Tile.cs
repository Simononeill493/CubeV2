using Microsoft.Xna.Framework.Graphics;

namespace CubeV2
{
    public class Tile
    {
        public bool Seen = false;
        public SpriteEffects flips = SpriteEffects.None;

        public Entity Contents { get; private set; }

        public Tile()
        {
            /*if (RandomUtils.RandomNumber(2) == 0)
            {
                flips = flips | SpriteEffects.FlipHorizontally;
            }
            if (RandomUtils.RandomNumber(2) == 0)
            {
                flips = flips | SpriteEffects.FlipVertically;
            }*/
        }

        public void SetContents(Entity e)
        {
            Contents = e;
        }
    }
}
