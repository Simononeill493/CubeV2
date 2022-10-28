using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class StoreDataInstruction : Instruction
    {
        public override string Name => "Store";

        public override int VariableCount => 1;

        public override int OutputCount => 1;

        public override Instruction GenerateNew() => new StoreDataInstruction();

        public override int Run(Entity caller, Board board)
        {
            Outputs[0] = Variables[0];
            return 0;
        }
    }
}
