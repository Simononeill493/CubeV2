using SAME;
using System;
using System.Data;

namespace CubeV2
{
    public abstract class Instruction
    {
        public abstract int VariableCount { get; }
        public IVariable[] Variables;

        public abstract int OutputCount { get; }
        public int[] OutputTargetVariables;

        public virtual int ControlOutputCount { get; } = 1;
        public int[] ControlFlowOutputs;

        public virtual int BaseEnergyCost { get; } = 0;

        public abstract string Name { get; }

        public Instruction()
        {
            Variables = new IVariable[Config.InstructionMaxNumVariables];
            for (int i = 0; i < Config.InstructionMaxNumVariables; i++)
            {
                Variables[i] = null;
            }

            OutputTargetVariables = new int[Config.InstructionMaxNumOutputs];
            ControlFlowOutputs = new int[Config.InstructionMaxNumControlFlowOutputs];

            for (int i = 0; i < Config.InstructionMaxNumOutputs; i++)
            {
                OutputTargetVariables[i] = -1;
            }

            for (int i = 0; i < Config.InstructionMaxNumControlFlowOutputs; i++)
            {
                ControlFlowOutputs[i] = -1;
            }
        }

        public abstract int Run(Entity caller, Board board);

        public abstract Instruction GenerateNew();

        public Instruction GenerateNewFilled()
        {
            var output = GenerateNew();

            throw new NotImplementedException();
            /*for (int i = 0; i < output.VariableCount; i++)
            {
                output.Variables[i] = RandomUtils.GetRandom(VariableOptionsGenerator.GetAllVariableOptions());
            }*/

            for (int j = 0; j < output.OutputCount; j++)
            {
                output.OutputTargetVariables[j] = RandomUtils.RandomNumber(Config.InstructionMaxNumOutputs + 1);
            }

            for (int k = 0; k < output.ControlOutputCount; k++)
            {
                output.ControlFlowOutputs[k] = RandomUtils.RandomNumber(Config.NumInstructionTiles - 1);
            }

            return output;
        }
    }
}
