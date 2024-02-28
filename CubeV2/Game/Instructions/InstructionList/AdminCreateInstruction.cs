using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class AdminCreateInstruction : AdminInstruction
    {
        public override string Name => "AdminCreate";
        public override int VariableCount => 2;
        public override int OutputCount => 0;
        public override int BaseEnergyCost { get; } = 0;

        public override Instruction GenerateNew() => new AdminCreateInstruction();

        public override int Run(Entity caller, Board board)
        {
            var location = Variables[0]?.Convert(caller, board, IVariableType.IntTuple);
            if (location == null)
            {
                return 0;
            }

            var tile = board.TryGetTile((Vector2Int)location);
            if (tile == null || tile.Contents != null)
            {
                return 0;
            }

            var template = Variables[1]?.Convert(caller, board, IVariableType.EntityType);
            if (template == null)
            {
                return 0;
            }

            var entity = ((EntityTemplate)template).GenerateEntity();
            entity.SetEnergyToMax();

            board.TryAddEntityToBoard(entity, (Vector2Int)location);
            return 0;
        }

    }
}
