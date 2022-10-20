namespace CubeV2
{
    public class MoveInstruction : Instruction
    {
        public override string Name => "Move";

        public override int VariableCount => 1;
        public override int OutputCount => 0;

        public MoveInstruction() : base() { }

        public MoveInstruction(RelativeDirection dir) : base()
        {
            Variables[0] = new StaticDirectionVariable(dir);
        }

        public override void Run(Entity caller)
        {
            var direction = Variables[0]?.Convert(caller, IVariableType.Direction);
            if (direction == null)
            {
                return;
            }

            caller.TryMove((RelativeDirection)direction);
        }

        public override Instruction GenerateNew() => new MoveInstruction();
    }


}
