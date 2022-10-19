﻿using CubeV2.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    static class DirectionUtils
    {
        public const int NumCardinalDirections = 8;
        public const int NumRelativeDirections = 8;

        public static List<CardinalDirection> Cardinals;
        public static List<RelativeDirection> Relatives;
        public static List<Orientation> Orientations;

        private static Dictionary<CardinalDirection, CardinalDirection> _reverseDict;
        private static Dictionary<CardinalDirection, Vector2Int> _XYOffsetDict;
        private static Dictionary<CardinalDirection, bool> _diagonalsDict;
        private static Dictionary<CardinalDirection, (CardinalDirection left, CardinalDirection right)> _parallelDict;

        private static Dictionary<RelativeDirection,string> _spritesDict;


        public static CardinalDirection Reverse(this CardinalDirection direction) => _reverseDict[direction];
        public static Vector2Int XYOffset(this CardinalDirection direction) => _XYOffsetDict[direction];
        public static bool IsDiagonal(this CardinalDirection dir) => _diagonalsDict[dir];
        public static (CardinalDirection Left, CardinalDirection Right) Parallel(this CardinalDirection dir) => _parallelDict[dir];
        public static Vector2Int GetAdjacentCoords(this Vector2Int coord, CardinalDirection direction) => coord + _XYOffsetDict[direction];

       

        public static void Init()
        {
            Cardinals = typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToList();
            Relatives = typeof(RelativeDirection).GetEnumValues().Cast<RelativeDirection>().ToList();
            Orientations = typeof(Orientation).GetEnumValues().Cast<Orientation>().ToList();

            _reverseDict = new Dictionary<CardinalDirection, CardinalDirection>
            {
                [CardinalDirection.North] = CardinalDirection.South,
                [CardinalDirection.South] = CardinalDirection.North,
                [CardinalDirection.West] = CardinalDirection.East,
                [CardinalDirection.East] = CardinalDirection.West,
                [CardinalDirection.NorthEast] = CardinalDirection.SouthWest,
                [CardinalDirection.SouthEast] = CardinalDirection.NorthWest,
                [CardinalDirection.NorthWest] = CardinalDirection.SouthEast,
                [CardinalDirection.SouthWest] = CardinalDirection.NorthEast
            };
            _XYOffsetDict = new Dictionary<CardinalDirection, Vector2Int>
            {
                [CardinalDirection.North] = Vector2Int.Up,
                [CardinalDirection.South] = Vector2Int.Down,
                [CardinalDirection.West] = Vector2Int.Left,
                [CardinalDirection.East] = Vector2Int.Right,
                [CardinalDirection.NorthEast] = new Vector2Int(1, -1),
                [CardinalDirection.SouthEast] = new Vector2Int(1, 1),
                [CardinalDirection.NorthWest] = new Vector2Int(-1, -1),
                [CardinalDirection.SouthWest] = new Vector2Int(-1, 1)
            };
            _diagonalsDict = new Dictionary<CardinalDirection, bool>
            {
                [CardinalDirection.North] = false,
                [CardinalDirection.South] = false,
                [CardinalDirection.West] = false,
                [CardinalDirection.East] = false,
                [CardinalDirection.NorthEast] = true,
                [CardinalDirection.SouthEast] = true,
                [CardinalDirection.NorthWest] = true,
                [CardinalDirection.SouthWest] = true
            };
            _parallelDict = new Dictionary<CardinalDirection, (CardinalDirection left, CardinalDirection right)>
            {
                [CardinalDirection.North] = (CardinalDirection.West, CardinalDirection.East),
                [CardinalDirection.South] = (CardinalDirection.East, CardinalDirection.West),
                [CardinalDirection.West] = (CardinalDirection.South, CardinalDirection.North),
                [CardinalDirection.East] = (CardinalDirection.North, CardinalDirection.South),
                [CardinalDirection.NorthEast] = (CardinalDirection.NorthWest, CardinalDirection.SouthEast),
                [CardinalDirection.SouthEast] = (CardinalDirection.NorthEast, CardinalDirection.SouthWest),
                [CardinalDirection.NorthWest] = (CardinalDirection.SouthWest, CardinalDirection.NorthEast),
                [CardinalDirection.SouthWest] = (CardinalDirection.SouthEast, CardinalDirection.NorthWest)
            };

            _spritesDict = new Dictionary<RelativeDirection, string>
            {
                [RelativeDirection.Forward] = DrawUtils.UpSprite,
                [RelativeDirection.Backward] = DrawUtils.DownSprite,
                [RelativeDirection.Left] = DrawUtils.LeftSprite,
                [RelativeDirection.Right] = DrawUtils.RightSprite,
                [RelativeDirection.ForwardRight] = DrawUtils.UpRightSprite,
                [RelativeDirection.BackwardRight] = DrawUtils.DownRightSprite,
                [RelativeDirection.ForwardLeft] = DrawUtils.UpLeftSprite,
                [RelativeDirection.BackwardLeft] = DrawUtils.DownLeftSprite
            };
        }

        public static List<(CardinalDirection, Vector2Int)> GetAdjacentCoords(Vector2Int coord)
        {
            return new List<(CardinalDirection, Vector2Int)>()
            {
                (CardinalDirection.North,new Vector2Int(coord.X+0,coord.Y-1)),
                (CardinalDirection.South,new Vector2Int(coord.X+0,coord.Y+1)),
                (CardinalDirection.West,new Vector2Int(coord.X-1,coord.Y+0)),
                (CardinalDirection.East,new Vector2Int(coord.X+1,coord.Y+0)),
                (CardinalDirection.NorthEast,new Vector2Int(coord.X+1,coord.Y-1)),
                (CardinalDirection.SouthEast,new Vector2Int(coord.X+1,coord.Y+1)),
                (CardinalDirection.NorthWest,new Vector2Int(coord.X-1,coord.Y-1)),
                (CardinalDirection.SouthWest,new Vector2Int(coord.X-1,coord.Y+1))
            };
        }

        public static CardinalDirection ApproachDirection(this Vector2Int self, Vector2Int other)
        {
            if (other.X > self.X)
            {
                if (other.Y > self.Y)
                {
                    return CardinalDirection.SouthEast;
                }
                else if (other.Y < self.Y)
                {
                    return CardinalDirection.NorthEast;
                }

                return CardinalDirection.East;
            }
            else if (other.X < self.X)
            {
                if (other.Y > self.Y)
                {
                    return CardinalDirection.SouthWest;
                }
                else if (other.Y < self.Y)
                {
                    return CardinalDirection.NorthWest;
                }

                return CardinalDirection.West;
            }
            else if (other.Y > self.Y)
            {
                return CardinalDirection.South;
            }
            return CardinalDirection.North;
        }
        public static CardinalDirection FleeDirection(this Vector2Int self, Vector2Int other) => ApproachDirection(self, other).Reverse();


        public static CardinalDirection ToCardinal(Orientation orientation, RelativeDirection relativeDirection) => (CardinalDirection)_underflowMod((int)relativeDirection + (int)orientation, 8);
        public static Orientation Rotate(this Orientation orientation, int rotation) => (Orientation)_underflowMod((int)orientation + rotation, 8);

        static int _underflowMod(int x, int m)
        {
            if (m == 0) { return x; }
            int res = x % m;

            return res >= 0 ? res : res + m;
        }

        public static string Sprite(this RelativeDirection dir) => _spritesDict[dir];
    }
}