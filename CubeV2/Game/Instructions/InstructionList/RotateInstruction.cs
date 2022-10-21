using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class RotateInstruction : Instruction
    {
        public override string Name => "Rotate";
        public override int VariableCount => 1;
        public override int OutputCount => 0;

        public override Instruction GenerateNew() => new RotateInstruction();

        public override void Run(Entity caller, Board board)
        {
            var rotationAmount = Variables[0]?.Convert(caller, IVariableType.Integer);
            if(rotationAmount!=null)
            {
                caller.Rotate((int)rotationAmount);
            }
        }
    }

    public enum RotationDirection
    {
        Left = -1,
        Right = 1
    }
}
