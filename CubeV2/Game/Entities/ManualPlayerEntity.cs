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
        HitInstruction _keyboardHitInstruction;


        public ManualPlayerEntity(string templateID, string entityID, string sprite) : base(templateID, entityID, sprite)
        {
            _keyboardMoveInstruction = new MoveInstruction();
            _keyboardHitInstruction = new HitInstruction();

        }

        public override void Tick(Board currentBoard,UserInput input)
        {
            var WASDDirection = DirectionUtils.GetWASDDirection(input);
            if(WASDDirection.AnyPressed)
            {
                _keyboardMoveInstruction.Variables[0] = new CardinalDirectionVariable(WASDDirection.Direction);
                _keyboardMoveInstruction.Run(this, currentBoard);
            }

            var arrowsDirection = DirectionUtils.GetArrowsDirection(input);
            if (arrowsDirection.AnyPressed)
            {
                _keyboardHitInstruction.Variables[0] = new CardinalDirectionVariable(arrowsDirection.Direction);
                _keyboardHitInstruction.Run(this, currentBoard);
            }
        }
    }
}
