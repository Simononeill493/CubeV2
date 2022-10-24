using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class IfInstruction : Instruction
    {
        public override string Name => "If";

        public override int VariableCount => 2;
        public override int OutputCount => 0;
        public override int ControlOutputCount => 2;

        public IOperator Operator { get; set; } = IOperator.Equals;
        public int IndexTrue => ControlOutputs[0];
        public int IndexFalse => ControlOutputs[1];

        public override Instruction GenerateNew() => new IfInstruction();

        public override void Run(Entity caller, Board board)
        {
            if(Variables[0]==null || Variables[1]==null)
            {
                return;
            }

            switch (Operator)
            {
                case IOperator.Equals:
                    if(Variables[0].IVariableEquals(caller,Variables[1]))
                    {
                        caller.SetInstructionCounter(IndexTrue-1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexFalse-1);
                    }
                    break;
            }
        }
    }

    public enum IOperator
    {
        Equals
    }
}
