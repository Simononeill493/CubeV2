using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (AnimationTracker.LaserActive)
            {
                var positions = AnimationTracker.LaserDirection;

                DrawUtils.DrawLine
                (
                    spriteBatch, 
                    position + _tileSizeTopOffset + (positions.Item1* _tileSizePadded), 
                    position + _tileSizeCenterOffset + (positions.Item2 * _tileSizePadded),
                    3,
                    Color.Red, 
                    Layer
                );
            }
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
        public static bool LaserActive;
        public static (Vector2Int, Vector2Int) LaserDirection;

        public static Dictionary<string, MovementCounter> EntityMovementTracker;

        public static void Reset()
        {
            LaserActive = false;
            LaserDirection = (Vector2Int.Zero, Vector2Int.Zero);

            EntityMovementTracker = new Dictionary<string, MovementCounter>();
        }

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
