namespace CubeV2
{
    public class VariableTileAppearance_Grid : VariableTileAppearance
    {
        public VariableTileAppearance_Grid(int gridIndex,int scale,float layer) : base(gridIndex,scale,layer){}

        public override IVariable GetSource() => GameInterface.GetVariableOption(Index);
    }

}
