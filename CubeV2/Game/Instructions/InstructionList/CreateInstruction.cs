using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    using System.Linq;
    using System.Runtime.CompilerServices;

    namespace CubeV2
    {
        public class CreateInstruction : Instruction
        {
            public override string Name => "Create";
            public override int VariableCount => 2;
            public override int OutputCount => 0;
            public override int BaseEnergyCost { get; } = Config.BaseCreateCost;

            public override Instruction GenerateNew() => new CreateInstruction();

            public CreateInstruction() : base() { }

            public CreateInstruction(RelativeDirection direction,EntityTemplate template) : base()
            {
                Variables[0] = new RelativeDirectionVariable(direction);
                Variables[1] = new EntityTypeVariable(template);
            }

            public CreateInstruction(IVariable direction, IVariable template) : base()
            {
                Variables[0] = direction;
                Variables[1] = template;
            }


            public override int Run(Entity caller, Board board)
            {
                var direction = Variables[0]?.Convert(caller, board, IVariableType.RelativeDirection);
                if(direction!=null)
                {
                    var targetTemplate = Variables[1]?.Convert(caller, board, IVariableType.EntityType);
                    if (targetTemplate != null)
                    {
                        var entity = ((EntityTemplate)targetTemplate).GenerateEntity();
                        entity.Orientation = caller.Orientation;

                        var trueDirection = DirectionUtils.ToCardinal(caller.Orientation,(RelativeDirection)direction);
                        var location = caller.Location + trueDirection.ToVector();

                        if (board.TryAddEntityToBoard(entity,location))
                        {
                            return Config.BaseCreateCost;

                        }
                        return 0;
                    }

                }

                return 0;
            }
        }
    }
}
