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
        private float _spriteMeterLayer;
        private float _spriteMeterLayer2;

        public GameTileAppearance(int gridIndex,float groundLayer,float spriteLayer,float spriteMeterLayer,float spriteMeterLayer2) : base(gridIndex, groundLayer) 
        {
            _spriteLayer = spriteLayer;
            _spriteMeterLayer = spriteMeterLayer;
            _spriteMeterLayer2 = spriteMeterLayer2;

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
                    //Tile background
                    DrawUtils.DrawTileSprite(spriteBatch, tile.Sprite, tile.Orientation,position, GameInterface.CameraScale, Layer, tile.Flips);
                    
                    if (tile.Contents != null)
                    {
                        //Entity in tile
                        DrawUtils.DrawTileSprite(spriteBatch, tile.Contents.Sprite,tile.Contents.Orientation, position, GameInterface.CameraScale, _spriteLayer, SpriteEffects.None);
                        if(tile.Contents.ShowHarvestMeter)
                        {
                            DrawUtils.DrawHarvestMeter(spriteBatch, tile.Contents.GetHarvestPercentage(), position, GameInterface.CameraScale, _spriteMeterLayer,_spriteMeterLayer2);
                        }
                    }
                }
                else
                {
                    //Covered by fog of war
                    DrawUtils.DrawRect(spriteBatch, position,GameInterface._cameraTileSizeFloat, Config.PlayerFogColor, Layer);
                }
            }
            else
            {
                //Tile doesn't exist
                DrawUtils.DrawRect(spriteBatch, position, GameInterface._cameraTileSizeFloat, Color.Black, Layer);
            }
        }
    }
}
