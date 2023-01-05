namespace CubeV2
{
    internal class AdminTakeEnergyInstruction : Instruction
    {
        public override string Name => "AdminTakeEnergy";
        public override int VariableCount => 2;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = 0;

        public override Instruction GenerateNew() => new AdminDropEnergyInstruction();

        public override int Run(Entity caller, Board board)
        {
            var location = Variables[0]?.Convert(caller, board, IVariableType.IntTuple);
            if (location == null)
            {
                return 0;
            }

            var tile = board.TryGetTile((Vector2Int)location);
            if (tile == null)
            {
                return 0;
            }

            var amount = Variables[1]?.Convert(caller, board, IVariableType.Integer);
            if (amount == null)
            {
                return 0;
            }

            caller.TryTakeEnergy(tile, (int)amount);
            return 0;
        }
    }
}
