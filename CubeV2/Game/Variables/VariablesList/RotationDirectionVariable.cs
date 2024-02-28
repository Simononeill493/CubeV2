using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
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

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.RotationDirection:
                case IVariableType.Integer:
                    return RotationDirection;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
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

            DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, str, position, Color.Green, scale / 3, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.RotationDirection);
            if (otherConverted != null)
            {
                return RotationDirection - (RotationDirection)otherConverted;
            }
            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => false;


    }

}
