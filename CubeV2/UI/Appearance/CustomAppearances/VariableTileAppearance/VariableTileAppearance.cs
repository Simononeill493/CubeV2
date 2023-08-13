using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{

    public abstract class VariableTileAppearance : TileAppearance
    {
        public override Vector2 Size => Vector2.Zero;
        private int _scale;

        public VariableTileAppearance(int gridIndex, int scale,float layer) : base(gridIndex,layer)
        {
            _scale = scale;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            var variable = GetSource();
            if(variable!=null)
            {
                variable.Draw(spriteBatch, position, _scale, Layer);
            }
        }

        public abstract IVariable GetSource();

    }

    public class VariableTileAppearance_ToSelect : VariableTileAppearance
    {
        public VariableTileAppearance_ToSelect(int gridIndex, int scale, float layer) : base(gridIndex, scale, layer)
        {}

        public override IVariable GetSource() => GameInterface.GetVariableFromGrid(Index);
    }

    public class VariableTileAppearanceFactoryGrid : TileAppearanceFactory
    {
        private int _scale;

        public VariableTileAppearanceFactoryGrid(float backgroundLayer, float foregroundLayer,int scale) : base(backgroundLayer, foregroundLayer)
        {
            _scale = scale;
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            var backgroundAppearance = new RectangleAppearance(width, height, Config.InstructionSelectorTileColor, _backgroundLayer);
            backgroundAppearance.OverrideColor(() => (GameInterface.IsFocusedOnInstructionOption(index) ? Color.Gray : Color.White));
            return backgroundAppearance;
        }

        public override TileAppearance CreateForeground(int index)
        {
            return new VariableTileAppearance_ToSelect(index,_scale,_foregroundLayer);
        }
    }



    public class VariableTileAppearance_InInstruction : VariableTileAppearance
    {
        private int _variableIndex;

        public VariableTileAppearance_InInstruction(int gridIndex, int variableIndex, int scale, float layer) : base(gridIndex, scale, layer)
        {
            _variableIndex = variableIndex;
        }

        public override IVariable GetSource() => GameInterface.GetInstructionVariable(Index, _variableIndex);
    }

}
