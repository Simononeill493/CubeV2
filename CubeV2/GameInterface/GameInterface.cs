using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2
{
    public partial class GameInterface
    {
        public static bool IsGameWon => _game.GameWon;
        public static string DisplayText = "Display text not yet set!";
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;

        public static Game _game;
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
        public static void RerollBoard()
        {
            _game.ResetBoardTemplate();
            DisplayText = "Board Rerolled!";
        }
        public static void ResetBoard()
        {
            _game.ResetBoard();
            DisplayText = "Board Reset!";
        }
        public static void StartBoard(TimeSpan updateRate)
        {
            _boardRunning = true;
            BoardUpdateRate = updateRate;
            TimeSinceLastUpdate = TimeSpan.Zero;

            DisplayText = "Running game...";
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

        public static void SimulateCurrentGame()
        {
            int timeout = 100;
            int iterations = 20;
            int setsToTest = 1000;

            DisplayText = "Simulating...";

            var bestInstructions = _focusedInstructions;
            int bestWins = 0;

            for(int i=0; i< int.MaxValue; i++)
            {
                var player = _game.CurrentTemplateTemplate.Entities.Where(e => e.TemplateID == EntityDatabase.PlayerName).First();
                player.Instructions = InstructionDatabase.GenerateRandom(Config.NumInstructionTiles);

                var wins = GameSimulator.Simulate(_game.CurrentTemplateTemplate, _game.WinCondition, timeout, iterations);
                if(wins>bestWins)
                {
                    bestInstructions = player.Instructions;
                    bestWins = wins;

                    if(bestWins>14)
                    {
                        break;
                    }
                }

                DisplayText = "Bestwins = " + bestWins + ".";

            }

            _focusedInstructions = bestInstructions;
            _game.CurrentTemplateTemplate.Entities.Where(e => e.TemplateID == EntityDatabase.PlayerName).First().Instructions = bestInstructions;
            DisplayText = "Bestwins = " + bestWins + ". Done.";

            //DisplayText = "Wins: " + wins + "/" + iterations + " (" + (wins / (float)iterations) + "%)";
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


    //TODO oh noooo
    public static class BoardCallback
    {
        public static GamePlaybackMode Mode = GamePlaybackMode.Headed;

        public static Board FocusedBoard => Mode == GamePlaybackMode.Headed ? HeadedBoard : HeadlessBoard;
        public static Board HeadedBoard => GameInterface._game.CurrentBoard;
        public static Board HeadlessBoard;

        public static bool TryMoveEntity(Entity e, Vector2Int location) => FocusedBoard.TryMoveEntity(e, location);
        public static Tile TryGetTile(int index) => FocusedBoard.TryGetTile(index);
        public static Tile TryGetTile(Vector2Int offset) => FocusedBoard.TryGetTile(offset);
        public static void ClearTile(Vector2Int targetLocation) => FocusedBoard.ClearThisTile(targetLocation);
    }

    public enum GamePlaybackMode
    {
        Headed,
        Headless
    }
}
