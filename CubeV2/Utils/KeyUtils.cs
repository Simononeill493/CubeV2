using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public static class KeyUtils
    {
        static Dictionary<Keys, char> _alphaNumericKeys;

        public static void Init()
        {
            _alphaNumericKeys = new Dictionary<Keys, char>();
            _alphaNumericKeys[Keys.OemMinus] = '-';
            _alphaNumericKeys[Keys.Space] = ' ';
            _alphaNumericKeys[Keys.OemPeriod] = '.';
            _alphaNumericKeys[Keys.OemComma] = ',';
        }

        public static char KeyToChar(Keys key)
        {
            var keyboard = Keyboard.GetState();
            var caps = CapsLock;
            var shift = keyboard.IsKeyDown(Keys.LeftShift);

            if (_alphaNumericKeys.Keys.Contains(key))
            {
                return _alphaNumericKeys[key];
            }

            var str = key.ToString();
            if (caps & !shift | !caps & shift)
            {
                str = str.ToUpper();
            }
            else
            {
                str = str.ToLower();
            }

            if (key.IsAlphabetical())
            {
                return str[0];
            }
            else if (key.IsNumeric())
            {
                return str[1];
            }
            else
            {
                return str[0];
            }
        }
        public static bool IsAlphabetical(this Keys key) => key >= Keys.A && key <= Keys.Z;
        public static bool IsNumeric(this Keys key) => key >= Keys.D0 && key <= Keys.D9;
        public static bool IsAlphanumeric(this Keys key) => key.IsAlphabetical() | key.IsNumeric();
        public static bool IsTypeable(this Keys key) => key.IsAlphanumeric() | _alphaNumericKeys.Keys.Contains(key);



        public static bool CapsLock => ((ushort)GetKeyState(0x14) & 0xffff) != 0;
        public static bool NumLock => ((ushort)GetKeyState(0x90) & 0xffff) != 0;
        public static bool ScrollLock => ((ushort)GetKeyState(0x91) & 0xffff) != 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

    }
}
