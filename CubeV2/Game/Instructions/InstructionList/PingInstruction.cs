using System.Linq;
using System.Runtime.CompilerServices;
using SAME; 

namespace CubeV2
{
    public class PingInstruction : Instruction
    {
        public override string Name => "Ping";
        public override int VariableCount => 1;
        public override int OutputCount => 1;
        public override int BaseEnergyCost { get; } = Config.BasePingCost;

        public override Instruction GenerateNew() => new PingInstruction();

        public override int Run(Entity caller, Board board)
        {
            var targetTemplate = Variables[0]?.Convert(caller, board, IVariableType.EntityType);
            if (targetTemplate != null)
            {
                var targetEntities = board.GetEntityByTemplate(((EntityTemplate)targetTemplate).TemplateID);
                var targetEntity = targetEntities.FirstOrDefault();
                if (targetEntity != null)
                {
                    caller.InstructionOutputs[0] = new CardinalDirectionVariable(caller.Location.ApproachDirection(targetEntity.Location));
                    return Config.BasePingCost;
                }
            }

            return 0;
        }
    }
}
