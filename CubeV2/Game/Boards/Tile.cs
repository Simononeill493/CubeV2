using Microsoft.Xna.Framework.Graphics;
using System;

namespace CubeV2
{
    public class Tile
    {
        public bool Seen = !Config.EnableFogOfWar;
        public string Sprite;
        public SpriteEffects Flips = SpriteEffects.None;
        public Orientation Orientation = Orientation.Top;

        public Entity Contents { get; private set; }

        public Tile()
        {

            Sprite = DrawUtils.GrassGround;

            /*
            Orientation = (Orientation)(RandomUtils.RandomNumber(4) * 2);

            if (RandomUtils.RandomNumber(2) == 0)
            {
                Sprite = DrawUtils.CircuitGround1;
            }
            else
            {
                Sprite = DrawUtils.CircuitGround2;
            }

            if (RandomUtils.RandomNumber(2) == 0)
            {
                Flips = Flips | SpriteEffects.FlipHorizontally;
            }
            if (RandomUtils.RandomNumber(2) == 0)
            {
                Flips = Flips | SpriteEffects.FlipVertically;
            }*/
        }

        public void SetContents(Entity e)
        {
            Contents = e;
        }

        public void Damage(int amount)
        {
            Contents?.TakeHealth(amount);
        }
    }
}
