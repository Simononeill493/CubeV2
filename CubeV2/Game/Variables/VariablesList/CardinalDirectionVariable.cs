using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override object Convert(Entity caller, IVariableType variableType)
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
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.PlayerSprite, position, scale, 0, Vector2.Zero, layer);
        }

        public override bool IVariableEquals(Entity caller, IVariable other)
        {
            var otherConverted = other.Convert(caller, IVariableType.CardinalDirection);
            if (otherConverted != null)
            {
                return (CardinalDirection)otherConverted == Direction;
            }
            return false;
        }

    }

}
