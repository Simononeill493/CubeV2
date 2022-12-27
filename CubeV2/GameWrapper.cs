using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Buffers;
using System.Linq;

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

            _previousInput = new UserInput(Mouse.GetState(), Mouse.GetState(), Keyboard.GetState(), Keyboard.GetState());
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _ui = UIBuilder.GenerateUI();
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

        protected override void Update(GameTime gameTime)
        {
            var input = new UserInput(Mouse.GetState(), _previousInput.MouseState, Keyboard.GetState(), _previousInput.KeyboardState);
            _universalKeybindings(input);

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
            if(input.KeysJustPressed.Any())
            {
                foreach (var element in AllUIElements.GetClickable)
                {
                    element.SendKeys(input);
                }

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
            GameInterface.Update(input,gameTime);

            _previousInput = input;
            _globalTickCount++;
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