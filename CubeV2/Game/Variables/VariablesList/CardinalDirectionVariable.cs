using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System.Collections.Generic;

namespace CubeV2
{
    public class CardinalDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.CardinalDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.Orientation };

        public CardinalDirection Direction { get; }

        public CardinalDirectionVariable(CardinalDirection direction)
        {
            Direction = direction;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RelativeDirection:
                case IVariableType.CardinalDirection:
                case IVariableType.Orientation:
                    return Direction;
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
            var otherConverted = other.Convert(caller, null, IVariableType.CardinalDirection);
            if (otherConverted != null)
            {
                return Direction - (CardinalDirection)otherConverted;
            }

            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;


    }

}
