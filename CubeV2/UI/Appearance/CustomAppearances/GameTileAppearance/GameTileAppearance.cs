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
                    DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite, position, GameInterface.CameraScale, 0, Vector2.Zero, Layer);
                    if (tile.Contents != null)
                    {
                        DrawUtils.DrawEntity(spriteBatch, tile.Contents, position, GameInterface.CameraScale, _spriteLayer);
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

                //DrawUtils.DrawSprite(spriteBatch, DrawUtils.VoidSprite, position, GameInterface.CameraScale, 0, Vector2.Zero, Layer);
            }
        }
    }
}
