using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
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

        public CapturedTileVariable(Vector2Int location, Entity contents)
        {
            Location = location;
            Contents = contents;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.CapturedTile:
                    return this;
                case IVariableType.IntTuple:
                    return Location;
                case IVariableType.EntityType:
                    if (Contents == null)
                    {
                        return null;
                    }
                    return EntityDatabase.Get(Contents.TemplateID);
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, CubeDrawUtils.SpritesDict[CubeDrawUtils.CircuitGround1], position, scale, 0, Vector2.Zero, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            switch (other.DefaultType)
            {
                case IVariableType.EntityType:
                    if (Contents == null)
                    {
                        return -1;
                    }
                    else if (((EntityTypeVariable)other)._template.TemplateID == Contents.TemplateID)
                    {
                        return 0;
                    }
                    break;
                case IVariableType.CapturedTile:
                    var otherCaptured = ((CapturedTileVariable)other);
                    if (otherCaptured.Location == Location && otherCaptured.Contents.TemplateID == Contents.TemplateID) //TODO: ?????
                    {
                        return 0;
                    }
                    break;
                case IVariableType.IntTuple:
                    if ((Vector2Int)other.Convert(caller, null, IVariableType.IntTuple) == Location)
                    {
                        return 0;
                    }
                    break;
                default:
                    return -1;
            }

            return -1;
        }

        public override bool IsEmpty(Entity caller, Board board) => Contents == null;

    }
}
