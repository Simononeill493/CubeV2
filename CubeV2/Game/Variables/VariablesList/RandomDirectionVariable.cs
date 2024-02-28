using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System.Collections.Generic;

namespace CubeV2
{
    public class RandomDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.RelativeDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.Orientation, IVariableType.Integer };

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RelativeDirection:
                case IVariableType.CardinalDirection:
                case IVariableType.Orientation:
                case IVariableType.Integer:
                    return RandomUtils.RandomRelative();
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, "RD", position, Color.Black, scale / 2, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.RelativeDirection);
            if (otherConverted != null)
            {
                return RandomUtils.RandomRelative() - (RelativeDirection)otherConverted;
            }

            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;

    }


}
