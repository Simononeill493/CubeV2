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
        public override int ControlOutputCount => 2;

        public override int BaseEnergyCost { get; } = Config.BaseRangePingCost;

        public int IndexFound { get => ControlFlowOutputs[0]; set => ControlFlowOutputs[0] = value; }
        public int IndexNotFound { get => ControlFlowOutputs[1]; set => ControlFlowOutputs[1] = value; }

        public override Instruction GenerateNew() => new PingRangeInstruction();


        public PingRangeInstruction() { }

        public PingRangeInstruction(EntityTemplate target,int range)
        {
            Variables[0] = new EntityTypeVariable(target);
            Variables[1] = new IntegerVariable(range);
        }

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

                                if(IndexFound>=0)
                                {
                                    caller.SetInstructionCounter(IndexFound - 1);
                                }
                                return Config.BaseRangePingCost * rangeInt;
                            }
                        }

                        if (IndexNotFound >= 0)
                        {
                            caller.SetInstructionCounter(IndexNotFound - 1);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
