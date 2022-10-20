namespace CubeV2
{
    public class PingInstruction : Instruction
    {
        public override string Name => "Ping";
        public override int VariableCount => 1;
        public override int OutputCount => 1;

        public override Instruction GenerateNew() => new PingInstruction();

        public override void Run(Entity caller)
        {
            throw new System.NotImplementedException();
        }
    }


}
