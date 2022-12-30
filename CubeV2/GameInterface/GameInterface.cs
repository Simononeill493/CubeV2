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

        public static void InitializeEmptyBoardGame() => _game = new EmptyBoardGame();
        public static void InitializeBoardlessGame() => _game = new EmptyBoardlessGame();

        public static void InitializeDemoFindGoalGame()
        {
            var demoPlayer = EntityDatabase.GetTemplate(EntityDatabase.AutoPlayerName);
            _focusedInstructions = demoPlayer.Instructions;

            _game = new DemoFindGoalGame(demoPlayer);
        }

        public static void InitializeBoardTest1Game()
        {
            var demoPlayer = EntityDatabase.GetTemplate(EntityDatabase.ManualPlayerName);
            _focusedInstructions = demoPlayer.Instructions;

            _game = new BoardTest1Game(demoPlayer);
            StartBoard(Config.BoardTest1UpdateRate);
        }

        

        public static void ManualSetNewBoard(Board b) => _game.SetNewBoard(b);
        public static void ManualUnsetBoard() => _game.UnsetBoard();

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
                    _game.TickBoard(input);

                    if (IsGameWon)
                    {
                        DisplayText = "A winner is you!";
                    }
                }
            }

            _processKeyboardShortcuts(input);
            _processMouseActions(input);
        }

        public static void SimulateCurrentGame(UserInput input)
        {
            int timeout = 100;
            int iterations = 200;

            DisplayText = "Simulating...";

            var wins = GameSimulator.Simulate(_game,input,_game.CurrentTemplateTemplate, _game.WinCondition, timeout, iterations);

            DisplayText = "Wins: " + wins + "/" + iterations;

            //DisplayText = "Wins: " + wins + "/" + iterations + " (" + (wins / (float)iterations) + "%)";
        }

        public static (int Current,int Max) GetPlayerEnergy()
        {
            if (_game != null && _game.CurrentBoard != null && _game.FocusEntity != null)
            {
                return (_game.FocusEntity.CurrentEnergy, _game.FocusEntity.MaxEnergy);
            }

            return (0,1);
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
