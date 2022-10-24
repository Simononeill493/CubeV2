namespace CubeV2
{
    public abstract class Instruction
    {
        public abstract int VariableCount { get; }
        public IVariable[] Variables;

        public abstract int OutputCount { get; }
        public IVariable[] Outputs;
        public int[] OutputTargets;


        public virtual int ControlOutputCount { get; } = 0;
        public int[] ControlOutputs;

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

            if (OutputCount > 0 && ControlOutputCount > 0)
            {
                throw new System.Exception("Can't (currently) have an instruction with both control and outputs.");
            }
        }

        public abstract void Run(Entity caller,Board board);

        public abstract Instruction GenerateNew();
    }
}
