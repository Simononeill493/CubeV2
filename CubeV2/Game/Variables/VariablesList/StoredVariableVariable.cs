using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class StoredVariableVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.StoredVariable;
        public override List<IVariableType> ValidTypes { get; } = VariableUtils.GetAllVariableTypes().ToList();

        public int VariableIndex { get; }

        public StoredVariableVariable(int variableIndex)
        {
            VariableIndex = variableIndex;
        }

        public override object Convert(Entity caller, IVariableType variableType)
        {
            var targetVariable = caller.Variables[VariableIndex];
            if (targetVariable != null)
            {
                return targetVariable.Convert(caller, variableType);
            }

            return null;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.PressStart2PFont, VariableIndex.ToString(), position, Color.Magenta, scale, layer);

        }
    }
}
