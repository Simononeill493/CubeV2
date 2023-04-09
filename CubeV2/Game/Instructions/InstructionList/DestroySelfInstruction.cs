using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    namespace CubeV2
    {
        public class DestroySelfInstruction : Instruction
        {
            public override string Name => "DestroySelf";

            public override int VariableCount => 0;
            public override int OutputCount => 0;

            public DestroySelfInstruction() : base() { }

            public override int Run(Entity caller, Board board)
            {
                board.TryClearThisTile(caller.Location);

                return 0;
            }

            public override Instruction GenerateNew() => new DestroySelfInstruction();
        }


    }
}
