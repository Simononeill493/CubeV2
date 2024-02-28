using SAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class ManualPlayerEntity : Entity
    {
        public Queue<Instruction> ManualInstructionBuffer = new Queue<Instruction>();

        private static MoveInstruction _keyboardMoveInstruction;
        private static PushDestroyInstruction _keyboardHitInstruction;
        private static PullEnergyInstruction _keyboardSapInstruction;

        public ManualPlayerEntity(string templateID, string entityID, string sprite) : base(templateID, entityID, sprite)
        {
            _keyboardMoveInstruction = new MoveInstruction();
            _keyboardHitInstruction = new PushDestroyInstruction();
            _keyboardSapInstruction = new PullEnergyInstruction();
        }

        public override void ExecuteInstructions(Board currentBoard, UserInput input)
        {
            if (GameInterface.PrimaryFocus == PrimaryFocus.Board)
            {
                var WASDDirection = DirectionUtils.GetWASDDirection(input);
                if (WASDDirection.AnyPressed)
                {
                    _keyboardMoveInstruction.Variables[0] = new CardinalDirectionVariable(WASDDirection.Direction);
                    _executeInstruction(_keyboardMoveInstruction, currentBoard);
                }

                var arrowsDirection = DirectionUtils.GetArrowsDirection(input);
                if (arrowsDirection.AnyPressed)
                {
                    _keyboardSapInstruction.Variables[0] = new CardinalDirectionVariable(arrowsDirection.Direction);
                    _executeInstruction(_keyboardSapInstruction, currentBoard);
                }

                if (ManualInstructionBuffer.Any())
                {
                    _executeInstruction(ManualInstructionBuffer.Dequeue(), currentBoard);
                }
            }

            base.ExecuteInstructions(currentBoard, input);

            //GiveEnergy(1000);
            if (GetCurrentEnergy() < 5)
            {
                GiveEnergy(1);
            }
        }
    }
}
