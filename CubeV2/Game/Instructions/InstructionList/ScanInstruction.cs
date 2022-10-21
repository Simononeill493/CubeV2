namespace CubeV2
{
    public class ScanInstruction : Instruction
    {
        public override string Name => "Scan";
        public override int VariableCount => 1;
        public override int OutputCount => 1;

        public override Instruction GenerateNew() => new ScanInstruction();

        public override void Run(Entity caller, Board board)
        {
            throw new System.NotImplementedException();
        }
    }


}
