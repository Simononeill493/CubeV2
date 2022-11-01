namespace CubeV2
{
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
