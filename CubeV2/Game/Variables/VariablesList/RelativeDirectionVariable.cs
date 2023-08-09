using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CubeV2
{
    public class RelativeDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.RelativeDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.Orientation, IVariableType.Integer };

        public RelativeDirection Direction { get; }

        public RelativeDirectionVariable(RelativeDirection direction)
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
                case IVariableType.Integer:
                    return Direction;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, Direction.Sprite(), position, scale, 0,Vector2.Zero, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller,null, IVariableType.RelativeDirection);
            if (otherConverted != null)
            {
                return Direction - (RelativeDirection)otherConverted;
            }
            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;


    }

}
