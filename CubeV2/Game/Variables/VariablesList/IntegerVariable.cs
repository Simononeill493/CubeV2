using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CubeV2
{
    public class IntegerVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.IntTuple;
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
            DrawUtils.DrawString(spriteBatch,DrawUtils.PressStart2PFont, Number.ToString(), position, Color.Black,scale-2, layer);
        }

        public override bool IVariableEquals(Entity caller, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.Integer);
            if (otherConverted != null)
            {
                return (int)otherConverted == Number;
            }
            return false;
        }
    }
}
