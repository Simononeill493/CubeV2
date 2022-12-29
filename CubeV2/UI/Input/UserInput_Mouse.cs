﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class UserInput
    {
        public MouseState MouseState;
        public Vector2 MousePos;
        public int MouseX;
        public int MouseY;

        public bool MouseLeftPressed;
        public bool MouseRightPressed;
        public bool MouseMiddlePressed;
        public bool MouseMoved;

        public bool MouseLeftReleased;
        public bool MouseRightReleased;
        public bool MouseMiddleReleased;

        public bool MouseLeftJustPressed;
        public bool MouseRightJustPressed;
        public bool MouseMiddleJustPressed;

        public bool MouseLeftJustReleased;
        public bool MouseRightJustReleased;
        public bool MouseMiddleJustReleased;

        public int ScrollDifference;
        public int ScrollDirection;

        protected void _setMouseState(MouseState newMouseState, MouseState oldMouseState)
        {
            MouseState = newMouseState;
            MouseX = MouseState.X;
            MouseY = MouseState.Y;
            MousePos = new Vector2(MouseX, MouseY);

            MouseLeftPressed = MouseState.LeftButton == ButtonState.Pressed;
            MouseRightPressed = MouseState.RightButton == ButtonState.Pressed;
            MouseMiddlePressed = MouseState.MiddleButton == ButtonState.Pressed;

            MouseLeftReleased = MouseState.LeftButton == ButtonState.Released;
            MouseRightReleased = MouseState.RightButton == ButtonState.Released;
            MouseMiddleReleased = MouseState.MiddleButton == ButtonState.Released;

            MouseLeftJustPressed = MouseLeftPressed & (oldMouseState.LeftButton == ButtonState.Released);
            MouseRightJustPressed = MouseRightPressed & (oldMouseState.RightButton == ButtonState.Released);
            MouseMiddleJustPressed = MouseMiddlePressed & (oldMouseState.MiddleButton == ButtonState.Released);

            MouseLeftJustReleased = MouseLeftReleased & (oldMouseState.LeftButton == ButtonState.Pressed);
            MouseRightJustReleased = MouseRightReleased & (oldMouseState.RightButton == ButtonState.Pressed);
            MouseMiddleJustReleased = MouseMiddleReleased & (oldMouseState.MiddleButton == ButtonState.Pressed);

            MouseMoved = (MouseX != oldMouseState.X) || (MouseY != oldMouseState.Y);

            ScrollDifference = MouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue;
            ScrollDirection += ScrollDifference > 0 ? 1 : ScrollDifference < 0 ? -1 : 0;
        }
    }
}
