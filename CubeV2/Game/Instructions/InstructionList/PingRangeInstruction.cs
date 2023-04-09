using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CubeV2
{
    public class PingRangeInstruction : Instruction
    {
        public override string Name => "PingRange";
        public override int VariableCount => 2;
        public override int OutputCount => 1;
        public override int BaseEnergyCost { get; } = Config.BasePingCost;

        public override Instruction GenerateNew() => new PingRangeInstruction();

        public override int Run(Entity caller, Board board)
        {
            throw new NotImplementedException();

            /*var targetTemplate = Variables[0]?.Convert(caller, board, IVariableType.EntityType);
            if (targetTemplate != null)
            {
                var targetEntities = board.GetEntityByTemplate(((EntityTemplate)targetTemplate).TemplateID);
                var targetEntity = targetEntities.FirstOrDefault();
                if (targetEntity != null)
                {
                    Outputs[0] = new CardinalDirectionVariable(caller.Location.ApproachDirection(targetEntity.Location));
                    return Config.BasePingCost;
                }
            }

            return 0;*/
        }
    }
}
