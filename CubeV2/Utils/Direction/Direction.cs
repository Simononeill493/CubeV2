using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    [Serializable()]
    public enum CardinalDirection
    {
        North = 0,
        NorthEast = 1,
        East = 2,
        SouthEast = 3,
        South = 4,
        SouthWest = 5,
        West = 6,
        NorthWest = 7
    }

    [Serializable()]
    public enum Orientation
    {
        Top = 0,
        TopRight = 1,
        Right = 2,
        BottomRight = 3,
        Bottom = 4,
        BottomLeft = 5,
        Left = 6,
        TopLeft = 7
    }

    [Serializable()]
    public enum RelativeDirection
    {
        Forward = 0,
        ForwardRight = 1,
        Right = 2,
        BackwardRight = 3,
        Backward = 4,
        BackwardLeft = 5,
        Left = 6,
        ForwardLeft = 7
    }

}
