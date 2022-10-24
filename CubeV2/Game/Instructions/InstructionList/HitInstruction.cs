namespace CubeV2
{
    public class HitInstruction : Instruction
    {
        public override string Name => "Hit";
        public override int VariableCount => 1;
        public override int OutputCount => 0;

        public override Instruction GenerateNew() => new HitInstruction();

        public override void Run(Entity caller, Board board)
        {
            var direction = Variables[0]?.Convert(caller, IVariableType.RelativeDirection);
            if (direction == null)
            {
                return;
            }

            caller.TryHit((RelativeDirection)direction);
        }
    }


}
