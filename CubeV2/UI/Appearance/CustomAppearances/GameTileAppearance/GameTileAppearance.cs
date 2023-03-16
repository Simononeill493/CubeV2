using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public class GameTileAppearance : TileAppearance
    {
        private float _spriteLayer;
        public GameTileAppearance(int gridIndex,float groundLayer,float spriteLayer) : base(gridIndex, groundLayer) 
        {
            _spriteLayer = spriteLayer;
        }

        public override Vector2 Size => GameInterface._cameraTileSizeFloat;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var loc = GameInterface.UITileGetRealTile(Index).realLocation;
            var tile = GameInterface._game.CurrentBoard.TryGetTile(loc);
            if(tile!=null)
            {
                if(tile.Seen)
                {
                    DrawUtils.DrawTileSprite(spriteBatch, tile.Sprite, tile.Orientation,position, GameInterface.CameraScale, Layer, tile.Flips);
                    if (tile.Contents != null)
                    {
                        DrawUtils.DrawTileSprite(spriteBatch, tile.Contents.Sprite,tile.Contents.Orientation, position, GameInterface.CameraScale, _spriteLayer, SpriteEffects.None);
                    }
                }
                else
                {
                    DrawUtils.DrawRect(spriteBatch, position,GameInterface._cameraTileSizeFloat, Config.PlayerFogColor, Layer);
                }
            }
            else
            {
                DrawUtils.DrawRect(spriteBatch, position, GameInterface._cameraTileSizeFloat, Color.Black, Layer);
                }
        }
    }
}
