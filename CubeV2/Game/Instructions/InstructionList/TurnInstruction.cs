using SAME;

namespace CubeV2
{
    public class TurnInstruction : Instruction
    {
        public override string Name => "TurnTo";
        public override int VariableCount => 1;
        public override int OutputCount => 0;

        public override Instruction GenerateNew() => new TurnInstruction();

        public TurnInstruction() : base() { }

        public TurnInstruction(CardinalDirection dir) : base()
        {
            Variables[0] = new CardinalDirectionVariable(dir);
        }

        public TurnInstruction(IVariable dir) : base()
        {
            Variables[0] = dir;
        }


        public override int Run(Entity caller, Board board)
        {
            var orientation = Variables[0]?.Convert(caller, board, IVariableType.Orientation);
            if (orientation != null)
            {
                caller.SetOrientation((Orientation)orientation);
            }

            return 0;
        }
    }
}
