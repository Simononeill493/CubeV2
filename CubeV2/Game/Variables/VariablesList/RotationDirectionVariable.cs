using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CubeV2
{
    public class RotationDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.RotationDirection;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.RotationDirection, IVariableType.Integer };

        public RotationDirection RotationDirection;

        public RotationDirectionVariable(RotationDirection rotationDirection)
        {
            RotationDirection = rotationDirection;
        }

        public override object Convert(Entity caller, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RotationDirection:
                    return RotationDirection;
                case IVariableType.Integer:
                    return (int)RotationDirection;

                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, IVariable variable, Vector2 position, int scale, float layer)
        {
            var str = "";
            if (RotationDirection == RotationDirection.Right)
            {
                str = "RGT";
            }
            else if (RotationDirection == RotationDirection.Left)
            {
                str = "LFT";
            }

            DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, str, position, Color.Green, scale / 3, layer);
        }
    }

}
