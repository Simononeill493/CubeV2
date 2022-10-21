using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CubeV2
{
    public class RandomDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.RelativeDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RelativeDirection, IVariableType.CardinalDirection, IVariableType.Orientation };

        public override object Convert(Entity caller, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RelativeDirection:
                case IVariableType.CardinalDirection:
                case IVariableType.Orientation:
                    return RandomUtils.RandomRelative();
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, "RD", position, Color.Black, scale / 2, layer);
        }
    }


}
