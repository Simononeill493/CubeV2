using CubeV2.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SAME;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;


namespace CubeV2
{
    public class CubeV2GameWrapper : GameWrapper
    {
        public CubeV2GameWrapper() : base((int)Config.ScreenSize.X, (int)Config.ScreenSize.Y) { }

        protected override void _initialize()
        {
            this.Window.Position = new Point(5, 30);
            // TODO: Add your initialization logic here
            _ui = UIBuilder.GenerateUI();
            //GameInterface.InitializeEmptyGame();
            //GameInterface.InitializeDemoFindGoalGame();
            //GameInterface.InitializeBoardTest1Game();
            GameInterface.InitializeFortressTutorial();
        }

        protected override void _loadContent()
        {
            CubeDrawUtils.LoadContent(GraphicsDevice, Content);
        }

        protected override void _update(UserInput input, GameTime gameTime)
        {
            var gameGrid = UIGameGrid.GetGameGrid();
            UIGameGrid.BoardPosition = gameGrid._position;

            _setCursorPosition(input, gameGrid.MouseOver);

            GameInterface.TryUpdate(input, gameTime);
        }

        protected override void _beginDraw(GameTime gameTime)
        {
            AnimationGifTracker.Update(gameTime.TotalGameTime);
            AnimationMovementTracker.TickEntityMovement(gameTime.ElapsedGameTime);
        }

        private void _setCursorPosition(UserInput input, bool isMouseOverGrid)
        {
            if (isMouseOverGrid)
            {
                AnimationCursorTracker.MousePos = input.MousePos;
                AnimationCursorTracker.CursorVisible = true;
                return;
            }

            AnimationCursorTracker.CursorVisible = false;
        }

        protected override void UniversalKeybindings(UserInput input)
        {
            if (input.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }


        protected override void _endDraw(GameTime gameTime) { }
    }
}