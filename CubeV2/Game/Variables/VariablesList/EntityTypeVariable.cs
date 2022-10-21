using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override object Convert(Entity caller, IVariableType variableType)
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
            DrawUtils.DrawSprite(spriteBatch, _template.Sprite, position, scale, 0, Vector2.Zero,layer);
        }
    }
}
