namespace CubeV2
{
    public abstract class Instruction
    {
        public IVariable[] Variables;
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
        }

        public abstract void Run(Entity caller);

        public abstract Instruction GenerateNew();
    }
}
