using CubeV2.Camera;
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

        public override Vector2 Size => GameCamera.TileSizeFloat;

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            var loc = GameCamera.GetBoardLocationFromCameraIndex(Index).boardLocation;
            var tile = GameInterface._game.CurrentBoard.TryGetTile(loc);
            if(tile!=null)
            {
                if(tile.Seen)
                {
                    //Tile background
                    DrawUtils.DrawTileSprite(spriteBatch, tile.Sprite, tile.Orientation,position, GameCamera.Scale, Layer, tile.Flips);
                    
                    if (tile.Contents != null)
                    {
                        _drawEntity(spriteBatch, position, tile.Contents);
                    }
                }
                else
                {
                    //Covered by fog of war
                    DrawUtils.DrawRect(spriteBatch, position, GameCamera.TileSizeFloat, Config.PlayerFogColor, Layer);
                }
            }
            else
            {
                //Tile doesn't exist
                DrawUtils.DrawRect(spriteBatch, position, GameCamera.TileSizeFloat, Color.Black, Layer);
            }
        }

        private void _drawEntity(SpriteBatch spriteBatch, Vector2 position,Entity entity)
        {
            if (entity.IsActive && AnimationMovementTracker.IsMoving(entity))
            {
                position -= (AnimationMovementTracker.GetMovementOffsetUnscaled(entity) * GameCamera.Scale);
            }

            //Entity
            DrawUtils.DrawTileSprite(spriteBatch, entity.Sprite, entity.Orientation, position, GameCamera.Scale, _spriteLayer, SpriteEffects.None);
            
            if (entity.ShowHarvestMeter)
            {
                //Harvest meter
                DrawUtils.DrawMeter(spriteBatch, entity.GetHarvestPercentage(), position, GameCamera.Scale, _spriteMeterLayer, _spriteMeterLayer2);
            }
            else if (entity.ShowDamageMeter)
            {
                //Harvest meter
                DrawUtils.DrawMeter(spriteBatch, entity.GetHealthPercentage(), position, GameCamera.Scale, _spriteMeterLayer, _spriteMeterLayer2);
            }

        }
    }
}
