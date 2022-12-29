namespace CubeV2
{
    public class HitInstruction : Instruction
    {
        public override string Name => "Hit";
        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = Config.BaseHitCost;

        public override Instruction GenerateNew() => new HitInstruction();

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller,board, IVariableType.RelativeDirection);
            if (direction == null)
            {
                return 0;
            }

            caller.Hit((RelativeDirection)direction);
            return Config.BaseHitCost;
        }
    }


}
