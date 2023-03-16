using Microsoft.Xna.Framework.Graphics;

namespace CubeV2
{
    public class Tile
    {
        public bool Seen = false;
        public string Sprite;
        public SpriteEffects Flips = SpriteEffects.None;
        public Orientation Orientation = Orientation.Top;

        public Entity Contents { get; private set; }

        public Tile()
        {
            //Orientation = Orientation.Right;
            //Sprite = DrawUtils.GroundSprite1;
            //Flips = Flips | SpriteEffects.FlipHorizontally;

            Orientation = (Orientation)(RandomUtils.RandomNumber(4) * 2);

            if (RandomUtils.RandomNumber(2) == 0)
            {
                Sprite = DrawUtils.GroundSprite1;
            }
            else
            {
                Sprite = DrawUtils.GroundSprite2;
            }

            if (RandomUtils.RandomNumber(2) == 0)
            {
                Flips = Flips | SpriteEffects.FlipHorizontally;
            }
            if (RandomUtils.RandomNumber(2) == 0)
            {
                Flips = Flips | SpriteEffects.FlipVertically;
            }
        }

        public void SetContents(Entity e)
        {
            Contents = e;
        }
    }
}
