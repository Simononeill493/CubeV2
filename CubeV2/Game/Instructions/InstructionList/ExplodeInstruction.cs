using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    namespace CubeV2
    {
        public class ExplodeInstruction : Instruction
        {
            public const int ExplosionDamage = 25;

            public override string Name => "Explode";

            public override int VariableCount => 0;
            public override int OutputCount => 0;

            public ExplodeInstruction() : base() { }

            public override int Run(Entity caller, Board board)
            {
                AnimationTracker.StartAnimation(DrawUtils.ExplosionGif, caller.Location.ToVector2(), TimeSpan.FromSeconds(0.03));

                foreach (var adjacent in caller.Location.GetAdjacentPoints())
                {
                    board.TryDamageTile(adjacent, ExplosionDamage);
                }

                caller.MarkForDeletion();
                return 0;
            }

            public override Instruction GenerateNew() => new ExplodeInstruction();
        }


    }
}
