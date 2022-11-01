using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        public static bool IsGameWon => _game.GameWon;
        public static string DisplayText = "Display text not yet set!";
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;

        private static Game _game;
        private static bool _boardRunning;

        public static void InitializeDemoGame()
        {
            var demoPlayer = EntityDatabase.GetTemplate(EntityDatabase.PlayerName);
            _focusedInstructions = demoPlayer.Instructions;

            _game = DemoGameGenerator.CreateDemoGame(demoPlayer);
        }
        public static void InitializeEmptyGame()
        {
            _game = new Game();
        }

        public static void ManualSetBoard(Board b) => _game.SetBoard(b);
        public static void RerollBoard() => _game.ResetBoardTemplate();
        public static void ResetBoard() => _game.ResetBoard();
        public static void StartBoard(TimeSpan updateRate)
        {
            _boardRunning = true;
            BoardUpdateRate = updateRate;
            TimeSinceLastUpdate = TimeSpan.Zero;
        }
        public static void TogglePauseBoard() => _boardRunning = !_boardRunning;
        public static void PauseBoard() => _boardRunning = false;

        public static void Update(UserInput input, GameTime gameTime)
        {
            if (_boardRunning)
            {
                TimeSinceLastUpdate += gameTime.ElapsedGameTime;
                if (TimeSinceLastUpdate >= BoardUpdateRate)
                {
                    TimeSinceLastUpdate = TimeSpan.Zero;
                    _game.TickBoard();

                    if (IsGameWon)
                    {
                        DisplayText = "A winner is you!";
                    }
                }
            }

            _processKeyboardShortcuts(input);
        }


       

        public static bool TryMoveEntity(Entity e, Vector2Int location) => _game.CurrentBoard.TryMoveEntity(e, location);

        public static Tile TryGetTile(int index) => _game.CurrentBoard.TryGetTile(index);
        public static Tile TryGetTile(Vector2Int offset) => _game.CurrentBoard.TryGetTile(offset);

        internal static void ClearTile(Vector2Int targetLocation)
        {
            _game.CurrentBoard.ClearThisTile(targetLocation);
        }

        public static int GetPlayerEnergy()
        {
            if (_game != null && _game.CurrentBoard != null)
            {
                var players = _game.CurrentBoard.GetEntityByTemplate(EntityDatabase.PlayerName);
                if (players.Any())
                {
                    return players.First().CurrentEnergy;
                }
            }

            return 0;
        }
    }
}
