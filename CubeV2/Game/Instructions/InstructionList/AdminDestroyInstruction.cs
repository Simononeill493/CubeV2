namespace CubeV2
{
    internal class AdminDestroyInstruction : Instruction
    {
        public override string Name => "AdminDestroy";
        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = 0;

        public override Instruction GenerateNew() => new AdminDestroyInstruction();

        public override int Run(Entity caller, Board board)
        {
            var location = Variables[0]?.Convert(caller, board, IVariableType.IntTuple);
            if (location == null)
            {
                return 0;
            }

            var tile = board.TryGetTile((Vector2Int)location);
            if (tile == null || tile.Contents == null)
            {
                return 0;
            }

            board.TryRemoveFromBoard(tile.Contents);
            return 0;
        }

    }
}
