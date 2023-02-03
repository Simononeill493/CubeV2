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
        public GamePadState ControllerState;
        public List<Buttons> ButtonsJustPressed;
        public List<Buttons> ButtonsJustReleased;

        private static List<Buttons> _buttonOptions = typeof(Microsoft.Xna.Framework.Input.Buttons).GetEnumValues().Cast<Buttons>().ToList();


        protected void _setControllerState(GamePadState newControllerState, GamePadState oldControllerState)
        {
            ControllerState = newControllerState;

            ButtonsJustPressed = new List<Buttons>();
            ButtonsJustReleased = new List<Buttons>();

            var previouslyPressed = _buttonOptions.Where(b => oldControllerState.IsButtonDown(b));
            var currentlyPressed = _buttonOptions.Where(b => newControllerState.IsButtonDown(b));

            foreach (var button in currentlyPressed)
            {
                if (!oldControllerState.IsButtonDown(button))
                {
                    ButtonsJustPressed.Add(button);
                }
            }

            foreach (var button in previouslyPressed)
            {
                if (!newControllerState.IsButtonDown(button))
                {
                    ButtonsJustReleased.Add(button);
                }
            }

            Console.WriteLine(string.Join(' ', currentlyPressed));
        }

        public bool IsButtonDown(Buttons button) => ControllerState.IsButtonDown(button);
        public bool IsButtonUp(Buttons button) => ControllerState.IsButtonUp(button);

        public bool IsButtonJustPressed(Buttons button) => ButtonsJustPressed.Contains(button);
        public bool IsButtonJustReleased(Buttons button) => ButtonsJustReleased.Contains(button);
    }
}
