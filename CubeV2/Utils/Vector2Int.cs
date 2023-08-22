using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    [Serializable()]
    public struct Vector2Int
    {
        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int One => new Vector2Int(1, 1);
        public static Vector2Int Two => new Vector2Int(2, 2);

        public static Vector2Int MinusOne = new Vector2Int(-1, -1);

        public static Vector2Int Up => new Vector2Int(0, -1);
        public static Vector2Int Down => new Vector2Int(0, 1);
        public static Vector2Int Left => new Vector2Int(-1, 0);
        public static Vector2Int Right => new Vector2Int(1, 0);

        public static Vector2Int MinValue => new Vector2Int(int.MinValue, int.MinValue);
        public static Vector2Int MaxValue => new Vector2Int(int.MaxValue, int.MaxValue);


        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2Int(Vector2 vector2) : this()
        {
            X = (int)vector2.X;
            Y = (int)vector2.Y;
        }

        public Vector2Int Absolute => new Vector2Int(Math.Abs(X), Math.Abs(Y));
        public Vector2Int ToOnes()
        {
            var x = X == 0 ? 0 : X > 0 ? 1 : -1;
            var y = Y == 0 ? 0 : Y > 0 ? 1 : -1;

            return new Vector2Int(x, y);
        }

        public static Vector2Int operator +(Vector2Int p) => p;
        public static Vector2Int operator -(Vector2Int p) => new Vector2Int(-p.X, -p.Y);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
        public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new Vector2Int(a.X * b.X, a.Y * b.Y);
        public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new Vector2Int(a.X / b.X, a.Y / b.Y);
        public static Vector2Int operator %(Vector2Int a, Vector2Int b) => new Vector2Int(a.X % b.X, a.Y % b.Y);

        public static Vector2 operator +(Vector2Int a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator +(Vector2 a, Vector2Int b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2Int a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 a, Vector2Int b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2Int a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator *(Vector2 a, Vector2Int b) => new Vector2(a.X * b.X, a.Y * b.Y);

        public static Vector2 operator %(Vector2 a, Vector2Int b) => new Vector2(a.X % b.X, a.Y % b.Y);


        public static Vector2 operator /(Vector2Int a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator /(Vector2 a, Vector2Int b) => new Vector2(a.X / b.X, a.Y / b.Y);

        public static Vector2Int operator +(Vector2Int p, int i) => new Vector2Int(p.X + i, p.Y + i);
        public static Vector2Int operator -(Vector2Int p, int i) => new Vector2Int(p.X - i, p.Y - i);
        public static Vector2Int operator *(Vector2Int p, int i) => new Vector2Int(p.X * i, p.Y * i);
        public static Vector2Int operator /(Vector2Int p, int i) => new Vector2Int(p.X / i, p.Y / i);
        public static Vector2 operator *(Vector2Int a, float b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator /(Vector2Int a, float b) => new Vector2(a.X / b, a.Y / b);

        public Vector2 ToVector2() => new Vector2(X, Y);

        public static bool operator ==(Vector2Int a, Vector2Int b) => a.Equals(b);
        public static bool operator !=(Vector2Int a, Vector2Int b) => !a.Equals(b);

        public int Max => Math.Max(X, Y);
        public int Min => Math.Min(X, Y);
        public int Product => X * Y;


        public int ManhattanDistance(Vector2Int other)
        {
            var distancePoint = (this - other).Absolute;
            var output = distancePoint.X + distancePoint.Y;
            return output;
        }

        public int EuclideanDistance(Vector2Int other)
        {
            var distancePoint = (this - other).Absolute;
            return distancePoint.Max;
        }


        public List<Vector2Int> GetAdjacentPoints()
        {
            return new List<Vector2Int>() {
                new Vector2Int(X+1, Y),
                new Vector2Int(X-1, Y),
                new Vector2Int(X, Y+1),
                new Vector2Int(X, Y-1),
                new Vector2Int(X+1, Y+1),
                new Vector2Int(X-1, Y-1),
                new Vector2Int(X+1, Y-1),
                new Vector2Int(X-1, Y+1)
            };
        }

        public static List<Vector2Int> GetRandomUniqueCoords(int width, int height,int number)
        {
            if (number > (width * height))
            {
                throw new Exception("Tried to get more unique coords than exist in the space");
            }

            var output = new List<Vector2Int>();
            var counter = 0;

            while(output.Count<number)
            {
                var newVector = new Vector2Int(RandomUtils.RandomNumber(width), RandomUtils.RandomNumber(height));
                if(!output.Contains(newVector))
                {
                    output.Add(newVector);
                }

                if(counter++ > 10000)
                {
                    Console.WriteLine("Warning: Took too long to generate unique coords. Going with what we have.");
                    return output;
                }
            }


            return output;

        }



        public override bool Equals(object obj) => obj is Vector2Int && Equals((Vector2Int)obj);
        public bool Equals(Vector2Int other) => X == other.X && Y == other.Y;
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }

        }
        public override string ToString() => X + " " + Y;


        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
        public bool InBoundsExclusive(int x, int y, int x2, int y2) => InBoundsInclusive(x + 1, y + 1, x2 - 1, y2 - 1);
        public bool InBoundsInclusive(int x, int y, int x2, int y2) => X >= x & X <= x2 & Y >= y & Y <= y2;

        private const char ParsingXYSeperator = ',';

        public static Vector2Int Parse(string s)
        {
            var splits = s.Split(ParsingXYSeperator);
            return new Vector2Int(int.Parse(splits[0]), int.Parse(splits[1]));
        }
    }
}
