using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class UserInput
    {
        public TimeSpan TimeSinceLastInput;

        public UserInput(MouseState mouseState, MouseState oldMouseState, KeyboardState keyboardState, KeyboardState oldKeyboardState,GamePadState controllerState, GamePadState oldControllerState, TimeSpan timeSinceLastInput)
        {
            _setMouseState(mouseState, oldMouseState);
            _setKeyboardState(keyboardState, oldKeyboardState);
            _setControllerState(controllerState, oldControllerState);

            TimeSinceLastInput = timeSinceLastInput;
        }
    }
}
