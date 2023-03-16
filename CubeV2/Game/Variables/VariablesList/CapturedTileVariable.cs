using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CubeV2
{
    public class CapturedTileVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.CapturedTile;

        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.CapturedTile, IVariableType.IntTuple };

        public Vector2Int Location;
        public Entity Contents;

        public CapturedTileVariable(Vector2Int location,Entity contents)
        {
            Location = location;
            Contents = contents;
        }

        public override object Convert(Entity caller,Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.CapturedTile:
                    return this;
                case IVariableType.IntTuple:
                    return Location;
                case IVariableType.EntityType:
                    if(Contents==null)
                    {
                        return null;
                    }
                    return EntityDatabase.GetTemplate(Contents.TemplateID);
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, DrawUtils.GroundSprite1, position, scale, 0, Vector2.Zero, layer);
        }

        public override bool IVariableEquals(Entity caller, IVariable other)
        {
            switch (other.DefaultType)
            {
                case IVariableType.EntityType:
                    if(Contents==null)
                    {
                        return false;
                    }
                    return ((EntityTypeVariable)other)._template.TemplateID == Contents.TemplateID;
                case IVariableType.CapturedTile:
                    var otherCaptured = ((CapturedTileVariable)other);
                    return (otherCaptured.Location == Location && otherCaptured.Contents.TemplateID == Contents.TemplateID);
                case IVariableType.IntTuple:
                    return (Vector2Int)other.Convert(caller, null, IVariableType.IntTuple) == Location;
                default:
                    return false;
            }
        }
    }
}
