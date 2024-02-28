using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System.Collections.Generic;

namespace CubeV2
{
    public class AgeVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.Integer;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.Integer, IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.RotationDirection };

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.Integer:
                case IVariableType.RelativeDirection:
                case IVariableType.CardinalDirection:
                case IVariableType.RotationDirection:
                    return board.Clock - caller.CreationTime;
                default:
                    return null;
            }
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.Integer);
            if (otherConverted != null)
            {
                return (board.Clock - caller.CreationTime) - (int)otherConverted;
            }

            return -1;
        }


        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, "AGE", position, Color.Black, scale - 2, layer);
        }

        public override bool IsEmpty(Entity caller, Board board) => false;
    }
}
