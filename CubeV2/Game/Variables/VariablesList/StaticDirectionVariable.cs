using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CubeV2
{
    public class StaticDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.RelativeDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RelativeDirection };

        public RelativeDirection Direction { get; }

        public StaticDirectionVariable(RelativeDirection direction)
        {
            Direction = direction;
        }

        public override object Convert(Entity caller, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RelativeDirection:
                    return Direction;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, IVariable variable, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, Direction.Sprite(), position, scale, 0,Vector2.Zero, layer);
        }

    }

}
