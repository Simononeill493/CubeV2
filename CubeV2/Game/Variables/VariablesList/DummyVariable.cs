using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class DummyVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType._null;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { };

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            return null;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawString(spriteBatch, DrawUtils.DefaultFont, "_null", position, Color.Black, scale - 2, layer);
        }

        public override bool IsEmpty(Entity caller, Board board)
        {
            return false;
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            return -1;
        }
    }
}
