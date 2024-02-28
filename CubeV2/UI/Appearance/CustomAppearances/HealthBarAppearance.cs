﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class HealthBarAppearance : Appearance
    {
        public override Vector2 Size { get; }

        private static Color BackgroundColor = Color.Gray;
        private static Color ForegroundColor = Color.Red;

        private float _frontLayer;

        public HealthBarAppearance(Vector2 size, float backLayer, float frontLayer) : base(backLayer)
        {
            Size = size;
            _frontLayer = frontLayer;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            DrawUtils.DrawRect(spriteBatch, position, Size, BackgroundColor, Layer);

            var percentage = GameInterface.GetPlayerHealthPercentage();
            DrawUtils.DrawRect(spriteBatch, position, Size * new Vector2(percentage, 1), ForegroundColor, _frontLayer);
        }
    }
}
