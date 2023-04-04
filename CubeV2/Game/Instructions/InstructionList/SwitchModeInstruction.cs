using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class SwitchModeInstruction : Instruction
    {
        public override string Name => "SwitchMode";

        public override int VariableCount => 1;
        public override int OutputCount => 0;
        public override int ControlOutputCount => 0;
        public override int BaseEnergyCost { get; } = Config.BaseMoveCost;

        public SwitchModeInstruction() : base() { }

        public SwitchModeInstruction(int index) : base()
        {
            Variables[0] = new IntegerVariable(index);
        }

        public override int Run(Entity caller, Board board)
        {
            var index = Variables[0]?.Convert(caller, board, IVariableType.Integer);
            if (index != null)
            {
                if (caller.TrySwitchInstructionSet((int)index))
                {
                    caller.SetInstructionCounter(Config.EntityMaxInstructionsPerSet + 1);
                    return 0;
                }
            }

            return 0;
        }

        public override Instruction GenerateNew() => new SwitchModeInstruction();
    }
}
