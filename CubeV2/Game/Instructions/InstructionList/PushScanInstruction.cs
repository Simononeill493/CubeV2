namespace CubeV2
{
    public class PushScanInstruction : Instruction
    {
        public override string Name => "PushScan";
        public override int VariableCount => 1;
        public override int OutputCount => 1;
        public override int BaseEnergyCost { get; } = Config.BaseScanCost;

        public override Instruction GenerateNew() => new PushScanInstruction();

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller, board, IVariableType.RelativeDirection);
            if (direction == null)
            {
                return 0;
            }

            var capturedTile = caller.TryPushScan((RelativeDirection)direction);
            Outputs[0] = capturedTile;
            return Config.BaseScanCost;
        }
    }


}
