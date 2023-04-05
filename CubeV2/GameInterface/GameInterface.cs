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
        public static PrimaryFocus PrimaryFocus = PrimaryFocus.Board;

        public static bool IsGameWon => _game.GameWon;
        public static string DisplayText = "Display text not yet set!";
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;

        public static Game _game;
        private static bool _boardRunning;

        private static Queue<Instruction> _manualInstructionBuffer = new Queue<Instruction>();

        public static void InitializeEmptyBoardGame() => _game = new EmptyBoardGame();
        public static void InitializeBoardlessGame() => _game = new EmptyBoardlessGame();

        public static void InitializeDemoFindGoalGame()
        {
            var demoPlayer = EntityDatabase.GetTemplate(EntityDatabase.AutoPlayerName);
            _focusedTemplate = EntityDatabase.GetTemplate(EntityDatabase.Ally2Name);

            _game = new DemoFindGoalGame(demoPlayer);
        }

        public static void InitializeBoardTest1Game()
        {
            var demoPlayer = EntityDatabase.GetTemplate(EntityDatabase.ManualPlayerName);
            _focusedTemplate = EntityDatabase.GetTemplate(EntityDatabase.Ally2Name);

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
            _manualInstructionBuffer.Clear();

            _game.ResetBoard();
            DisplayText = "Board Reset!";
        }
        public static void StartBoard(TimeSpan updateRate)
        {
            SetCameraConfig(Config.DefaultZoomLevel);

            _boardRunning = true;
            BoardUpdateRate = updateRate;
            TimeSinceLastUpdate = TimeSpan.Zero;
        }
        public static void TogglePauseBoard() => _boardRunning = !_boardRunning;
        public static void PauseBoard() => _boardRunning = false;

        public static void TryUpdate(UserInput input, GameTime gameTime)
        {
            _processKeyboardActions(input);
            _processMouseActions(input);

            if (_boardRunning)
            {
                TimeSinceLastUpdate += gameTime.ElapsedGameTime;
                if (TimeSinceLastUpdate >= BoardUpdateRate)
                {
                    _update(input);
                }
            }

        }

        private static void _update(UserInput input)
        {
            TimeSinceLastUpdate = TimeSpan.Zero;

            if (_manualInstructionBuffer.Any())
            {
                //Console.WriteLine("Instruction was removed from queue.");
                input.ManualClickInstruction = _manualInstructionBuffer.Dequeue();
            }

            _game.TickBoard(input);

            if(!IsPlayerInCamera())
            {
                CenterCameraOnPlayer();
            }

            RevealMapToPlayer();

            if (IsGameWon)
            {
                DisplayText = "A winner is you!";
            }

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
                return (_game.FocusEntity.GetCurrentEnergy(), _game.FocusEntity.MaxEnergy);
            }

            return (0,1);
        }

        public static Vector2Int GetFocusedTileCoordinates()
        {
            if (FocusedTileExists)
            {
                return BoardUtils.IndexToXY(_focusedTile, _game.CurrentBoard._width);
            }

            return Vector2Int.MinusOne;
        }
        public static bool FocusedTileHasEntity()
        {
            if (FocusedTileExists)
            {
                return _game.CurrentBoard.TryGetTile(_focusedTile).Contents != null;
            }

            return false;
        }
        public static Entity GetFocusedTileEntity()
        {
            if (FocusedTileHasEntity())
            {
                return _game.CurrentBoard.TryGetTile(_focusedTile).Contents;
            }

            return null;
        }

        public static bool TileExists(int tileIndex)
        {
            if (_game != null && _game.CurrentBoard != null)
            {
                var tile = _game.CurrentBoard.TryGetTile(tileIndex);
                if (tile != null)
                {
                    return true;
                }
            }

            return false;
        }
    }


    //TODO oh noooo
    public static class EntityBoardCallback
    {
        public static GamePlaybackMode Mode = GamePlaybackMode.Headed;

        public static Board FocusedBoard => Mode == GamePlaybackMode.Headed ? HeadedBoard : HeadlessBoard;
        public static Board HeadedBoard => GameInterface._game.CurrentBoard;
        public static Board HeadlessBoard;

        public static bool TryCreate(Entity entity, Vector2Int location) => FocusedBoard.AddEntityToBoard(entity,location);
        public static bool TryMove(Entity e, Vector2Int location) => FocusedBoard.TryMoveEntity(e, location);
        public static Tile TryGetTile(int index) => FocusedBoard.TryGetTile(index);
        public static Tile TryGetTile(Vector2Int offset) => FocusedBoard.TryGetTile(offset);
        public static bool TryClearTile(Vector2Int targetLocation) => FocusedBoard.TryClearThisTile(targetLocation);
    }

    public enum GamePlaybackMode
    {
        Headed,
        Headless
    }

    public enum PrimaryFocus
    {
        Board,
        Editor
    }

}
