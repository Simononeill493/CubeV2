namespace CubeV2
{
    public class TurnInstruction : Instruction
    {
        public override string Name => "TurnTo";
        public override int VariableCount => 1;
        public override int OutputCount => 0;

        public override Instruction GenerateNew() => new TurnInstruction();

        public override void Run(Entity caller)
        {
            throw new System.NotImplementedException();
        }
    }


}
