using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
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


        public int IndexTrue { get => ControlFlowOutputs[0]; set => ControlFlowOutputs[0] = value; }
        public int IndexFalse { get => ControlFlowOutputs[1]; set => ControlFlowOutputs[1] = value; }

        public override Instruction GenerateNew() => new IfInstruction();

        public IfInstruction() { }

        public IfInstruction(IVariable v1, IVariable v2)
        {
            Variables[0] = v1;
            Variables[1] = v2;
        }


        public override int Run(Entity caller, Board board)
        {
            if(Variables[0]==null || Variables[1]==null)
            {
                return 0;
            }

            switch (Operator)
            {
                case IOperator.Equals:
                    if(Variables[0].IVariableCompare(caller, board, Variables[1]) == 0)
                    {
                        caller.SetInstructionCounter(IndexTrue-1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexFalse-1);
                    }
                    break;
                case IOperator.NotEquals:
                    if (Variables[0].IVariableCompare(caller, board, Variables[1]) == 0)
                    {
                        caller.SetInstructionCounter(IndexFalse - 1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexTrue - 1);
                    }
                    break;
                case IOperator.MoreThan:
                    if (Variables[0].IVariableCompare(caller, board, Variables[1]) > 0)
                    {
                        caller.SetInstructionCounter(IndexTrue - 1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexFalse - 1);
                    }
                    break;
                case IOperator.LessThan:
                    if (Variables[0].IVariableCompare(caller, board, Variables[1]) < 0)
                    {
                        caller.SetInstructionCounter(IndexTrue - 1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexFalse - 1);
                    }
                    break;
                case IOperator.IsEmpty:
                    if (Variables[0].IsEmpty(caller, board))
                    {
                        caller.SetInstructionCounter(IndexTrue - 1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexFalse - 1);
                    }
                    break;
                case IOperator.NotEmpty:
                    if (Variables[0].IsEmpty(caller, board))
                    {
                        caller.SetInstructionCounter(IndexFalse - 1);
                    }
                    else
                    {
                        caller.SetInstructionCounter(IndexTrue - 1);
                    }
                    break;
            }

            return 0;
        }
    }

    public enum IOperator
    {
        Equals,
        NotEquals,
        MoreThan,
        LessThan,
        IsEmpty,
        NotEmpty
    }
}
