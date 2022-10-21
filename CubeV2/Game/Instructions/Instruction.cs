namespace CubeV2
{
    public abstract class Instruction
    {
        public IVariable[] Variables;

        public int[] OutputTargets;
        public IVariable[] Outputs;

        public abstract string Name { get; }

        public abstract int VariableCount { get; }
        public abstract int OutputCount { get; }

        public Instruction()
        {
            Variables = new IVariable[Config.InstructionMaxNumVariables];
            for (int i = 0; i < Config.InstructionMaxNumVariables; i++)
            {
                Variables[i] = null;
            }

            OutputTargets = new int[Config.InstructionMaxNumOutputs];
            Outputs = new IVariable[Config.InstructionMaxNumOutputs];

            for (int i = 0; i < Config.InstructionMaxNumOutputs; i++)
            {
                OutputTargets[i] = -1;
                Outputs[i] = null;
            }
        }

        public abstract void Run(Entity caller,Board board);

        public abstract Instruction GenerateNew();
    }
}
