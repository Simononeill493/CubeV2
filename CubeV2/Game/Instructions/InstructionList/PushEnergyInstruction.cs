namespace CubeV2
{
    internal class PushEnergyInstruction : Instruction
    {
        public override string Name => "PushEnergy";
        public override int VariableCount => 2;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = 0;

        public override Instruction GenerateNew() => new PushEnergyInstruction();

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller, board, IVariableType.RelativeDirection);
            if (direction != null)
            {
                var amount = Variables[1]?.Convert(caller, board, IVariableType.Integer);
                if(amount!=null)
                {
                    if (caller.TryPushEnergy(board, (RelativeDirection)direction,(int)amount))
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }
    }
}
