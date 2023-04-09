namespace CubeV2
{
    public class PullEnergyInstruction : Instruction
    {
        public override string Name => "PullEnergy";
        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = 0;

        public override Instruction GenerateNew() => new PullEnergyInstruction();

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller, board, IVariableType.RelativeDirection);
            if (direction != null)
            {
                if (caller.TryPullEnergy(board, (RelativeDirection)direction))
                {
                    return 0;
                }
            }

            return 0;
        }
    }
}
