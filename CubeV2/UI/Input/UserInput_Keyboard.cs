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
        public KeyboardState KeyboardState;
        public List<Keys> KeysJustPressed;
        public List<Keys> KeysJustReleased;
        public string CharactersPressed = "";

        public bool IsNumberJustPressed => KeysJustPressed.Any(k => k.IsNumeric());
        public int GetNumberJustPressed => KeysJustPressed.FirstOrDefault(k => k.IsNumeric()).KeyToInt();

        protected void _setKeyboardState(KeyboardState newKeyboardState, KeyboardState oldKeyboardState)
        {
            KeyboardState = newKeyboardState;
            KeysJustPressed = new List<Keys>();
            KeysJustReleased = new List<Keys>();

            var previouslyPressed = oldKeyboardState.GetPressedKeys();
            var currentlyPressed = newKeyboardState.GetPressedKeys();

            foreach (var key in currentlyPressed)
            {
                if (!oldKeyboardState.IsKeyDown(key))
                {
                    KeysJustPressed.Add(key);
                }
            }

            foreach (var key in previouslyPressed)
            {
                if (!newKeyboardState.IsKeyDown(key))
                {
                    KeysJustReleased.Add(key);
                }
            }

            foreach(var key in KeysJustPressed)
            {
                if(key.IsTypeable())
                {
                    CharactersPressed += KeyUtils.KeyToChar(key);
                }
            }
        }

        public bool IsKeyDown(Keys key) => KeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => KeyboardState.IsKeyUp(key);

        public bool IsKeyJustPressed(Keys key) => KeysJustPressed.Contains(key);
        public bool IsKeyJustReleased(Keys key) => KeysJustReleased.Contains(key);

        public void RemoveKeyJustPressed(Keys key) => KeysJustPressed.Remove(key);
        public void RemoveKeyJustReleased(Keys key) => KeysJustReleased.Remove(key);
    }
}
