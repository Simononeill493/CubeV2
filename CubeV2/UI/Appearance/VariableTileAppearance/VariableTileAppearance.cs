using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class VariableTileAppearanceFactory_ForSelectionGrid : TileAppearanceFactory
    {
        private int _scale;

        public VariableTileAppearanceFactory_ForSelectionGrid(int scale,float layer) : base(layer)
        {
            _scale = scale;
        }

        public override TileAppearance Create(int index)
        {
            return new VariableTileAppearance_Grid(index, _scale,_layer);
        }
    }

    public abstract class VariableTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;

        public VariableTileAppearance(int gridIndex, int scale,float layer) : base(gridIndex,layer)
        {
            _scale = scale;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var variable = GetSource();
            if(variable!=null)
            {
                variable.Draw(spriteBatch, position, _scale, _layer);
            }
        }

        public abstract IVariable GetSource();

    }

    public class VariableTileAppearance_Grid : VariableTileAppearance
    {
        public VariableTileAppearance_Grid(int gridIndex,int scale,float layer) : base(gridIndex,scale,layer){}

        public override IVariable GetSource() => GameInterface.GetVariableOption(Index);
    }

    public class VariableTileAppearance_Instruction : VariableTileAppearance
    {
        private int _variableIndex;

        public VariableTileAppearance_Instruction(int slotIndex, int variableIndex,int scale,float layer) : base(slotIndex,scale,layer) 
        {
            _variableIndex = variableIndex;
        }

        public override IVariable GetSource() => GameInterface.GetVariable(Index, _variableIndex);
    }

}
