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

            _previousInput = new UserInput(Mouse.GetState(), Mouse.GetState(), Keyboard.GetState(), Keyboard.GetState(), GamePad.GetState(0), GamePad.GetState(0));
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _ui = UIBuilder.GenerateUI();
            //GameInterface.InitializeEmptyGame();
            //GameInterface.InitializeDemoFindGoalGame();
            GameInterface.InitializeBoardTest1Game();

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
            var input = new UserInput(Mouse.GetState(), _previousInput.MouseState, Keyboard.GetState(), _previousInput.KeyboardState, GamePad.GetState(0),_previousInput.ControllerState);

            //Console.WriteLine(input.MousePos);
            _universalKeybindings(input);

            if (input.MouseMoved)
            {
                foreach (var element in AllUIElements.GetMouseInteractionElements)
                {
                    element.CheckMouseOver(input.MousePos);
                }
            }

            if (input.MouseLeftJustPressed)
            {
                foreach (var element in AllUIElements.GetClickable)
                {
                    element.TryLeftClick(input);
                }
            }
            if (input.MouseRightJustPressed)
            {
                foreach (var element in AllUIElements.GetClickable)
                {
                    element.TryRightClick(input);
                }
            }
            if (input.KeysJustPressed.Any())
            {
                foreach (var element in AllUIElements.GetTypeable)
                {
                    element.SendKeys(input);
                }
            }

            var gameGrid = AllUIElements.GetUIElement(Config.GameGridName);

            _setCursorTilePosition(input, gameGrid);
            if (Config.EnablePlayerRangeOverlay)
            {
                _setOperationalRangeOverlayPosition(gameGrid);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
            GameInterface.TryUpdate(input,gameTime);

            _previousInput = input;
            _globalTickCount++;

        }

        private void _setCursorTilePosition(UserInput input,UIElement gameGrid)
        {
            if (gameGrid.MouseOver)
            {
                var cursorTile = AllUIElements.GetUIElement(Config.CursorOverlayTileName);

                var gridShrinkFactor = GameInterface.CameraTileSize;
                var startPos = new Vector2Int(input.MousePos - gameGrid._position);

                var rescaledPos = (startPos / gridShrinkFactor) * gridShrinkFactor;
                cursorTile.SetOffset(rescaledPos.ToVector2());
            }
        }

        private void _setOperationalRangeOverlayPosition(UIElement gameGrid)
        {
            var focusEntity = GameInterface._game.FocusEntity;
            if (focusEntity!=null)
            {
                var rangeOverlay = AllUIElements.GetUIElement(Config.OperationalRangeOverlayTileName);
                var offset = ((focusEntity.Location - GameInterface.CameraOffset - (Config.PlayerOperationalRadius)) * GameInterface._cameraTileSizeFloat);

                rangeOverlay.SetOffset(offset);
            }
        }


        private void _universalKeybindings(UserInput input)
        {
            if (input.IsKeyDown(Keys.Escape) & (input.IsKeyDown(Keys.LeftShift) | (input.IsKeyDown(Keys.RightShift))))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if(DoDraw)
            {
                GraphicsDevice.Clear(Config.PrimaryBackgroundColor);

                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                _ui.Draw(_spriteBatch, Vector2.Zero);
                _spriteBatch.End();

                base.Draw(gameTime);


                // TODO: Add your drawing code here

                base.Draw(gameTime);
            }
        }
    }
}