using System.Linq;

namespace CubeV2
{
    public class PingInstruction : Instruction
    {
        public override string Name => "Ping";
        public override int VariableCount => 1;
        public override int OutputCount => 1;

        public override Instruction GenerateNew() => new PingInstruction();

        public override void Run(Entity caller, Board board)
        {
            var targetTemplate = Variables[0]?.Convert(caller, IVariableType.EntityType);
            if(targetTemplate != null)
            {
                var targetEntities = board.LocateEntityType(((EntityTemplate)targetTemplate).TemplateID);
                var targetEntity = targetEntities.FirstOrDefault();
                if(targetEntity!=null)
                {
                    Outputs[0] = new CardinalDirectionVariable(caller.Location.ApproachDirection(targetEntity.Location));
                }
            }
        }
    }
}
