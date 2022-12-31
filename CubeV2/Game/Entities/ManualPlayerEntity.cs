using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class ManualPlayerEntity : Entity
    {
        MoveInstruction _keyboardMoveInstruction;
        PushDestroyInstruction _keyboardHitInstruction;
        PullEnergyInstruction _keyboardSapInstruction;

        public ManualPlayerEntity(string templateID, string entityID, string sprite) : base(templateID, entityID, sprite)
        {
            _keyboardMoveInstruction = new MoveInstruction();
            _keyboardHitInstruction = new PushDestroyInstruction();
            _keyboardSapInstruction = new PullEnergyInstruction();
        }

        public override void Tick(Board currentBoard,UserInput input)
        {
            var WASDDirection = DirectionUtils.GetWASDDirection(input);
            if(WASDDirection.AnyPressed)
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

            base.Tick(currentBoard, input);

            //GiveEnergy(1000);
            if(CurrentEnergy < 5)
            {
                GiveEnergy(1);
            }
        }
    }
}
