using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class LocationVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.IntTuple;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.IntTuple };

        public Vector2Int Location { get; }

        public LocationVariable(Vector2Int location)
        {
            Location = location;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.IntTuple:
                    return Location;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, CubeDrawUtils.SpritesDict[CubeDrawUtils.PlayerSprite], position, scale, 0, Vector2.Zero, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.IntTuple);
            if (otherConverted != null && Location == (Vector2Int)otherConverted)
            {
                return 0;
            }
            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;

    }
}
