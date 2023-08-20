using CubeV2.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class BoardAnimator : Appearance
    {
        public override Vector2 Size => _size;

        private Vector2 _size;
        private Vector2Int _tileSize;
        private Vector2Int _tileSizePadded;
        private Vector2Int _tileSizeTopOffset;
        private Vector2Int _tileSizeCenterOffset;

        private int _indexWidth;
        private int _indexHeight;
        private int _tileWidth;
        private int _tileHeight;
        private int _padding;

        public BoardAnimator(float layer) : base(layer) {}

        public static void Reset()
        {
            AnimationTracker.Animations = new List<AnimationTracker.BoardAnimation>();

            AnimationMovementTracker.EntityMovementTracker = new Dictionary<string, MovementCounter>();

            AnimationLaserTracker.LaserActive = false;
            AnimationLaserTracker.LaserLocation = (Vector2.Zero, Vector2.Zero);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            AnimationLaserTracker.Draw(spriteBatch, position, _tileSizeTopOffset, _tileSizeCenterOffset, _tileSizePadded, Layer);

            var boardOffsetScaled = (GameCamera.IndexOffset * Config.TileBaseSize) * GameCamera.Scale;

            AnimationTracker.Draw(spriteBatch, gameTime.TotalGameTime, position,boardOffsetScaled, GameCamera.Scale, DrawUtils.BoardAnimationLayer);
        }

        public void Arrange(Vector2 size,int indexWidth,int indexHeight,int tileWidth,int tileHeight,int padding)
        {
            _size = size;
            _indexWidth = indexWidth;
            _indexHeight = indexHeight;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _padding = padding;

            _tileSize = new Vector2Int(_tileWidth, _tileHeight);
            _tileSizePadded = new Vector2Int(_tileWidth+_padding, _tileHeight+_padding);
            _tileSizeTopOffset = new Vector2Int(_tileWidth / 2, 0);
            _tileSizeCenterOffset = _tileSize / 2;
        }
    }

    public static class AnimationTracker
    {
        public static List<BoardAnimation> Animations;
        public static TimeSpan CurrentTime;

        public static void Update(TimeSpan currentTime)
        {
            CurrentTime = currentTime;
        }

        public static void Draw(SpriteBatch spriteBatch, TimeSpan currentTime, Vector2 position,Vector2 cameraOffset, int scale, float layer)
        {
            var toRemove = new List<BoardAnimation>();

            foreach(var animation in Animations)
            {
                var elapsed = currentTime - animation._startTime;
                var frame = elapsed / animation._stepLength;

                if(frame>= animation._gif.NumFrames)
                {
                    toRemove.Add(animation);
                    continue;
                }

                animation.Draw(spriteBatch, (int)frame, position,cameraOffset, scale, layer);
            }

            foreach(var finishedAnimation in toRemove)
            {
                Animations.Remove(finishedAnimation);
            }
        }

        internal static void StartAnimation(string gifName, Vector2 boardPositionUnscaled, TimeSpan frameLength)
        {
            Animations.Add(new BoardAnimation(DrawUtils.GifDict[gifName], boardPositionUnscaled, CurrentTime, frameLength));
        }

        public class BoardAnimation
        {
            public CustomGifClass _gif;
            public Vector2 _boardPositionUnscaled;
            public TimeSpan _startTime;
            public TimeSpan _stepLength;

            private float _transparency = 1;

            public BoardAnimation(CustomGifClass gif, Vector2 boardPositionUnscaled, TimeSpan startTime, TimeSpan stepLength)
            {
                _gif = gif;
                _boardPositionUnscaled = boardPositionUnscaled;
                _startTime = startTime;
                _stepLength = stepLength;
            }

            public void Draw(SpriteBatch spriteBatch,int frame,Vector2 position,Vector2 cameraOffset,int scale,float layer)
            {
                Vector2 actualPosition = position-cameraOffset + (_boardPositionUnscaled * scale *Config.TileBaseSize);

                _gif.Draw(spriteBatch, frame, actualPosition, Vector2.One * scale, 0, Vector2.Zero, layer, Color.White * _transparency);
            }
        }
    }

    public static class AnimationLaserTracker
    {
        public static bool LaserActive;
        public static (Vector2, Vector2) LaserLocation;

        public static void SetLaserLocation(Vector2Int playerLocation,Vector2Int targetLocation)
        {
            LaserLocation = (playerLocation.ToVector2(), targetLocation.ToVector2());
        }

        public static void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2Int tileSizeTopOffset, Vector2Int tileSizeCenterOffset, Vector2Int tileSizePadded, float layer)
        {
            if (LaserActive)
            {
                Vector2 laserStart = (position + tileSizeTopOffset + (LaserLocation.Item1 * tileSizePadded));
                Vector2 laserEnd = position + tileSizeCenterOffset + (LaserLocation.Item2 * tileSizePadded);

                DrawUtils.DrawLine
                (
                    spriteBatch,
                    laserStart,
                    laserEnd,
                    3,
                    Color.Red,
                    layer
                );
            }
        }
    }

    public static class AnimationMovementTracker
    {
        public static Dictionary<string, MovementCounter> EntityMovementTracker;

        public static void AddEntityMovement(string id,int updateRate,Vector2Int approachVector)
        {
            EntityMovementTracker[id] = new MovementCounter(approachVector, updateRate * Config.BoardMasterUpdateRate);
        }

        public static bool IsMoving(Entity e) => EntityMovementTracker.ContainsKey(e.EntityID);

        public static Vector2 GetMovementOffsetUnscaled(Entity entity)
        {
            var movementData = EntityMovementTracker[entity.EntityID];
            var movementPercentage = movementData.Remaining / movementData.Total;

            var offset = movementData.Direction * Config.TileBaseSize.X * (float)movementPercentage;

            return offset;
        }

        public static void TickEntityMovement(TimeSpan timeSinceLastDraw)
        {
            var toRemove = new List<string>();

            foreach(var kvp in EntityMovementTracker)
            {
                kvp.Value.Remaining -= timeSinceLastDraw;
                if (kvp.Value.Remaining <= TimeSpan.Zero)
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach(var e in toRemove)
            {
                EntityMovementTracker.Remove(e);
            }
        }
    }

    public class MovementCounter
    {
        public Vector2Int Direction;

        public TimeSpan Total;
        public TimeSpan Remaining;

        public MovementCounter(Vector2Int direction, TimeSpan total)
        {
            Direction = direction;
            Total = total;
            Remaining = total;
        }
    }
}
