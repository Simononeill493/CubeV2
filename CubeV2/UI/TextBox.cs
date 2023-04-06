using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace CubeV2
{
    internal partial class UIBuilder
    {
        public class TextBox : UIElement
        {
            public string Text = "";
            public bool Editable = true;

            public TextBox(string id,int width, int height,float boxLayer,float textLayer) : base(id)
            {
                AddAppearance(new RectangleAppearance(width, height, Color.White, boxLayer));
                AddAppearance(new TextAppearance(Color.Black, textLayer, () => Text));

                AddKeyPressedAction(CheckKeyPress);
            }

            public void CheckKeyPress(UserInput input)
            {
                if (!Editable)
                {
                    return;
                }

                var alphaNumPressed = input.KeysJustPressed.Where(k => k.IsAlphanumeric());
                foreach (var key in alphaNumPressed)
                {
                    if (IsKeyValid(key))
                    {
                        Text += key.KeyToChar();
                    }
                }

                if(input.IsKeyJustPressed(Keys.Back) && Text.Length>0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
            }

            public Func<Keys, bool> IsKeyValid = (s) => true;
        }
    }
}
