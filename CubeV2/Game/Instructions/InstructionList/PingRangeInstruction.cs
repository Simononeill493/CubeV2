using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CubeV2
{
    public class PingRangeInstruction : Instruction
    {
        public override string Name => "PingRange";
        public override int VariableCount => 2;
        public override int OutputCount => 1;
        public override int BaseEnergyCost { get; } = Config.BaseRangePingCost;

        public override Instruction GenerateNew() => new PingRangeInstruction();

        public override int Run(Entity caller, Board board)
        {
            var range = Variables[1]?.Convert(caller, board, IVariableType.Integer);
            if (range != null)
            {
                var rangeInt = (int)range;
                if(!caller.HasEnergy(Config.BaseRangePingCost * rangeInt))
                {
                    return 0;
                }

                var targetTemplate = Variables[0]?.Convert(caller, board, IVariableType.EntityType);
                {
                    if(targetTemplate!=null)
                    {
                        foreach (var tile in board.GetSurroundings(caller.Location, rangeInt))
                        {
                            if (tile.Item1.Contents != null && tile.Item1.Contents.TemplateID==((EntityTemplate)targetTemplate).TemplateID)
                            {
                                caller.InstructionOutputs[0] = new CardinalDirectionVariable(caller.Location.ApproachDirection(tile.Item1.Contents.Location));
                                return Config.BaseRangePingCost * rangeInt;
                            }
                        }

                    }
                }
            }

            return 0;
        }
    }
}
