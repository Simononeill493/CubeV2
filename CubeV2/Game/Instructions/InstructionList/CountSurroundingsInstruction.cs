using System.Linq;
using System.Runtime.CompilerServices;

namespace CubeV2
{
    public class CountSurroundingsInstruction : Instruction
    {
        public override string Name => "CountSurroundings";
        public override int VariableCount => 0;
        public override int OutputCount => 1;
        public override int BaseEnergyCost { get; } = Config.BaseCountSurroundingsCost;

        public override Instruction GenerateNew() => new CountSurroundingsInstruction();

        public override int Run(Entity caller, Board board)
        {
            var count = caller.Location.GetAdjacentPoints().Select(p => board.TryGetTile(p)?.Contents).Where(c=>c!=null).Count();
            Outputs[0] = new IntegerVariable(count);

            return Config.BaseCountSurroundingsCost;
        }
    }
}
