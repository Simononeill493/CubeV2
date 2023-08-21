using CubeV2.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;


namespace CubeV2
{
    public class GameWrapper : Microsoft.Xna.Framework.Game
    {
        public static bool DoDraw = true;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private UIElement _ui;
        private UserInput _previousInput;

        private int _globalTickCount = 0;

        public GameWrapper()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = (int)Config.ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)Config.ScreenSize.Y;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //this.IsFixedTimeStep = true;//false;
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);

            _previousInput = new UserInput(Mouse.GetState(), Mouse.GetState(), Keyboard.GetState(), Keyboard.GetState(), GamePad.GetState(0), GamePad.GetState(0),TimeSpan.Zero);
        }

        protected override void Initialize()
        {
            this.Window.Position = new Point(5, 30);
            // TODO: Add your initialization logic here
            _ui = UIBuilder.GenerateUI();
            //GameInterface.InitializeEmptyGame();
            //GameInterface.InitializeDemoFindGoalGame();
            //GameInterface.InitializeBoardTest1Game();
            GameInterface.InitializeFortressTutorial();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DrawUtils.LoadContent(GraphicsDevice,Content);
            // TODO: use this.Content to load your game content here
        }


        //TODO: Hm. 

        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine("UpdateDelta:" + gameTime.ElapsedGameTime);
            var input = new UserInput(Mouse.GetState(), _previousInput.MouseState, Keyboard.GetState(), _previousInput.KeyboardState, GamePad.GetState(0),_previousInput.ControllerState,gameTime.ElapsedGameTime);

            _universalKeybindings(input);

            if (input.MouseMoved)
            {
                foreach (var element in AllUIElements.GetMouseInteractionElements)
                {
                    element.CheckMouseOver(input.MousePos);
                }
            }

            if(input.MousePressed)
            {
                var pressableElements = AllUIElements.GetPressableWithMouseOver.ToList();

                if (input.MouseLeftPressed)
                {
                    foreach (var element in pressableElements)
                    {
                        element.PressLeft(input);
                    }
                }
                if (input.MouseRightPressed)
                {
                    foreach (var element in pressableElements)
                    {
                        element.PressRight(input);
                    }
                }

                if (input.MouseJustPressed)
                {
                    var clickableElements = AllUIElements.GetClickableWithMouseOver.ToList();

                    if (input.MouseLeftJustPressed)
                    {
                        foreach (var element in clickableElements)
                        {
                            element.LeftClick(input);
                        }
                    }
                    if (input.MouseRightJustPressed)
                    {
                        foreach (var element in clickableElements)
                        {
                            element.RightClick(input);
                        }
                    }
                }
            }

            if (input.KeyPressed)
            {
                foreach (var element in AllUIElements.GetTypeable)
                {
                    element.SendKeys(input);
                }
            }

            var gameGrid = (UIGameGrid)AllUIElements.GetUIElement(Config.GameGridName);
            _setCursorPosition(input, gameGrid);
            _setOperationalRangeOverlayPosition(gameGrid);

            // TODO: Add your update logic here

            base.Update(gameTime);
            GameInterface.TryUpdate(input,gameTime);

            _previousInput = input;
            _globalTickCount++;

        }

        private void _setCursorPosition(UserInput input,UIGameGrid gameGrid)
        {
            if (gameGrid.MouseOver)
            {
                AnimationCursorTracker.GridLocation = gameGrid._position;
                AnimationCursorTracker.MousePos = input.MousePos;
                AnimationCursorTracker.CursorVisible = true;
            }
            else
            {
                AnimationCursorTracker.CursorVisible = false;
            }

        }

        private void _setOperationalRangeOverlayPosition(UIElement gameGrid)
        {
            if(!Config.EnablePlayerRangeOverlay)
            {
                return;
            }

            var focusEntity = GameInterface._game.FocusEntity;
            if (focusEntity!=null)
            {
                var rangeOverlay = AllUIElements.GetUIElement(Config.OperationalRangeOverlayTileName);
                var offset = ((focusEntity.Location - GameCamera.IndexOffset - (Config.PlayerRangeLimit)) * GameCamera.TileSizeFloat) - GameCamera.SubTileOffset;

                rangeOverlay.SetOffset(offset);
            }
        }


        private void _universalKeybindings(UserInput input)
        {
            if (input.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            //Console.WriteLine("DrawDelta:" + gameTime.ElapsedGameTime);
            //return;

            AnimationGifTracker.Update(gameTime.TotalGameTime);
            AnimationMovementTracker.TickEntityMovement(gameTime.ElapsedGameTime);

            if (DoDraw)
            {
                GraphicsDevice.Clear(Config.PrimaryBackgroundColor);

                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                _ui.Draw(_spriteBatch, Vector2.Zero,gameTime);
                _spriteBatch.End();

                base.Draw(gameTime);
            }


            //Console.WriteLine("Sprites:" + DrawUtils.SpritesDrawn);
            //DrawUtils.SpritesDrawn = 0;
        }
    }
}