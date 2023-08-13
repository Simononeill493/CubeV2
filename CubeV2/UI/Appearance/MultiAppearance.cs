using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class MultiAppearance : Appearance
    {
        public override Vector2 Size => Appearances.First().Size;
        public List<Appearance> Appearances = new List<Appearance>();

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            foreach(var appearance in Appearances)
            {
                appearance.Draw(spriteBatch, position,gameTime);
            }
        }

        public static MultiAppearance Create(params Appearance[] appearances)
        {
            var multi = new MultiAppearance();
            multi.Appearances = appearances.ToList();

            return multi;
        }

        private MultiAppearance() : base(0) {}
    }
}
