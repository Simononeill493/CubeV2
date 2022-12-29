using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    /*public class LocationVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.IntTuple;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.IntTuple, IVariableType.Tile };

        public Vector2Int Location { get; }

        public LocationVariable(Vector2Int location)
        {
            location = Location;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.IntTuple:
                    return Location;
                case IVariableType.Tile:
                    return board.TryGetTile(Location);
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
            var otherConverted = other.Convert(caller, null, IVariableType.IntTuple);
            if (otherConverted != null)
            {
                return (Vector2Int)otherConverted == Location;
            }
            return false;
        }

    }*/
}
