using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CubeV2
{
    public class CapturedTileVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.Tile;

        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.Tile };

        public Vector2Int Location;
        public Entity Contents;

        public CapturedTileVariable(Vector2Int location,Entity contents)
        {
            Location = location;
            Contents = contents;
        }

        public override object Convert(Entity caller, IVariableType variableType)
        {
            return null;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite, position, scale, 0, Vector2.Zero, layer);
        }

        public override bool IVariableEquals(Entity caller, IVariable other)
        {
            if(other.DefaultType== IVariableType.EntityType && Contents!=null)
            {
                return ((EntityTypeVariable)other)._template.TemplateID == Contents.TemplateID;
            }

            return false;
        }
    }
}
