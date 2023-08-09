using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CubeV2
{
    public abstract class IVariable
    {
        public abstract IVariableType DefaultType { get; }
        public abstract List<IVariableType> ValidTypes { get; }

        public abstract object Convert(Entity caller,Board board,IVariableType variableType);

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer);

        public abstract int IVariableCompare(Entity caller, Board board, IVariable other);

        public abstract bool IsEmpty(Entity caller, Board board);
    }
}
