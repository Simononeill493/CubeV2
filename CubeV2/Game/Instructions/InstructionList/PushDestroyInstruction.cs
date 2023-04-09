namespace CubeV2
{
    public class PushDestroyInstruction : Instruction
    {
        public override string Name => "PushDestroy";
        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = Config.BaseHitCost;

        public override Instruction GenerateNew() => new PushDestroyInstruction();

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller,board, IVariableType.RelativeDirection);
            if (direction == null)
            {
                return 0;
            }

            caller.PushDestroy(board,(RelativeDirection)direction);
            return Config.BaseHitCost;
        }
    }
}
