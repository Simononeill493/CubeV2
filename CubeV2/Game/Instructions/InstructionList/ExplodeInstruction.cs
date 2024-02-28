using SAME;
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
            public const int ExplosionDamageMin = 25;
            public const int ExplosionDamageMax = 35;

            public override string Name => "Explode";

            public override int VariableCount => 0;
            public override int OutputCount => 0;

            public ExplodeInstruction() : base() { }

            public override int Run(Entity caller, Board board)
            {
                AnimationGifTracker.StartAnimation(CubeDrawUtils.ExplosionGif, caller.Location.ToVector2(), TimeSpan.FromSeconds(0.03));

                foreach (var adjacent in caller.Location.GetAdjacentPoints())
                {
                    board.TryDamageTile(adjacent, RandomUtils.RandomNumber(25, 45));
                }

                caller.MarkForDeletion();
                return 0;
            }

            public override Instruction GenerateNew() => new ExplodeInstruction();
        }


    }
}
