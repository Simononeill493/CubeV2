using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System.Collections.Generic;

namespace CubeV2
{
    public class IntegerVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.Integer;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.Integer, IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.RotationDirection };

        public int Number { get; }

        public IntegerVariable(int number)
        {
            Number = number;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.Integer:
                case IVariableType.RelativeDirection:
                case IVariableType.CardinalDirection:
                case IVariableType.RotationDirection:
                    return Number;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, Number.ToString(), position, Color.Black, scale - 2, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.Integer);
            if (otherConverted != null)
            {
                return Number - (int)otherConverted;
            }
            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;

    }
}
