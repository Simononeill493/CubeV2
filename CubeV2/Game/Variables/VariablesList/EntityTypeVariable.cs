﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class EntityTypeVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.EntityType;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.EntityType };

        public EntityTemplate _template;

        public EntityTypeVariable(EntityTemplate template)
        {
            _template = template;
        }

        public override object Convert(Entity caller, Board board, IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.EntityType:
                    return _template;
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int scale, float layer)
        {
            DrawUtils.DrawSprite(spriteBatch, CubeDrawUtils.SpritesDict[_template.DisplaySprite], position, scale, 0, Vector2.Zero, layer);
        }

        public override int IVariableCompare(Entity caller, Board board, IVariable other)
        {
            var otherConverted = other.Convert(caller, null, IVariableType.EntityType);
            if (otherConverted != null && ((EntityTemplate)otherConverted).TemplateID == _template.TemplateID)
            {
                return 0;
            }

            return -1;
        }


        public override bool IsEmpty(Entity caller, Board board) => _template == null;

    }
}
