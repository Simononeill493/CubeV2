namespace CubeV2
{
    public class MoveInstruction : Instruction
    {
        public override string Name => "Move";

        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = Config.BaseMoveCost;

        public MoveInstruction() : base() { }

        public MoveInstruction(RelativeDirection dir) : base()
        {
            Variables[0] = new RelativeDirectionVariable(dir);
        }

        public override int Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller, IVariableType.RelativeDirection);
            if (direction != null)
            {
                if (caller.TryMove((RelativeDirection)direction))
                {
                    return Config.BaseMoveCost;
                }
            }

            return 0;
        }

        public override Instruction GenerateNew() => new MoveInstruction();
    }


}
