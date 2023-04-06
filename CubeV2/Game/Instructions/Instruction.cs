using System;
using System.Data;

namespace CubeV2
{
    public abstract class Instruction
    {
        public abstract int VariableCount { get; }
        public IVariable[] Variables;

        public abstract int OutputCount { get; }
        public IVariable[] Outputs;
        public int[] OutputTargets;

        public virtual int ControlOutputCount { get; } = 1;
        public int[] ControlOutputs;

        public virtual int BaseEnergyCost { get; } = 0;

        public abstract string Name { get; }

        public Instruction()
        {
            Variables = new IVariable[Config.InstructionMaxNumVariables];
            for (int i = 0; i < Config.InstructionMaxNumVariables; i++)
            {
                Variables[i] = null;
            }

            OutputTargets = new int[Config.InstructionMaxNumOutputs];
            Outputs = new IVariable[Config.InstructionMaxNumOutputs];
            ControlOutputs = new int[Config.InstructionMaxNumControlOutputs];

            for (int i = 0; i < Config.InstructionMaxNumOutputs; i++)
            {
                OutputTargets[i] = -1;
                Outputs[i] = null;
            }

            for (int i = 0; i < Config.InstructionMaxNumControlOutputs; i++)
            {
                ControlOutputs[i] = -1;
            }
        }

        public abstract int Run(Entity caller,Board board);

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
                output.OutputTargets[j] = RandomUtils.RandomNumber(Config.InstructionMaxNumOutputs+1);
            }

            for (int k = 0; k < output.ControlOutputCount; k++)
            {
                output.ControlOutputs[k] = RandomUtils.RandomNumber(Config.NumInstructionTiles - 1);
            }

            return output;
        }
    }
}
