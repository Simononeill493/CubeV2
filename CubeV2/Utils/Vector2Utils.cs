using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2.Utils
{
    public static class Vector2Utils
    {
        public static string ToStringRounded(this Vector2 v,int digits) => Math.Round(v.X,digits) + " " + Math.Round(v.Y, digits);

        public static Vector2Int Ceiled(this Vector2 v) => new Vector2Int((int)Math.Ceiling(v.X), (int)Math.Ceiling(v.Y));
        public static Vector2Int Floored(this Vector2 v) => new Vector2Int((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        public static Vector2Int Rounded(this Vector2 v) => new Vector2Int((int)Math.Round(v.X), (int)Math.Round(v.Y));

        public static Vector2 Clamped(this Vector2 v,Vector2 low,Vector2 high)
        {
            var output = new Vector2(v.X, v.Y);

            if (output.X > high.X)
            {
                output.X = high.X;
            }
            if (output.Y > high.Y)
            {
                output.Y = high.Y;
            }

            if (output.X < low.X)
            {
                output.X = low.X;
            }
            if (output.Y < low.Y)
            {
                output.Y = low.Y;
            }

            return output;
        }
    }
}
