﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class OutputSelectionTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;

        public OutputSelectionTileAppearance(int gridIndex, int scale, float layer) : base(gridIndex, layer)
        {
            _scale = scale;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, Index.ToString(), position, Color.Magenta, _scale, Layer);
        }
    }

}
