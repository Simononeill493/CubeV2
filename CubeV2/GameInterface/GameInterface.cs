using CubeV2.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2
{
    internal class GameInterface
    {
        public static List<IVariable> VariableOptions = new List<IVariable>();
        public static List<Instruction> SelectableInstructions => _game?.KnownInstructions;

        public static List<Instruction> FocusedInstructions;



        public static int FocusedInstruction = -1;
        public static int FocusedVariable = -1;


        public static void AssignValueToFocusedVariable(int optionIndex)
        {
            if(FocusedVariableExists & (optionIndex < VariableOptions.Count))
            {
                var newVariable = VariableOptions[optionIndex];
                FocusedInstructions[FocusedInstruction].Variables[FocusedVariable] = newVariable;
            }
        }

        internal static void AssignValueToFocusedInstruction(int selectableInstructionIndex)
        {
            if(SelectableInstructionExists(selectableInstructionIndex) & InstructionExists(FocusedInstruction))
            {
                var toSet = SelectableInstructions[selectableInstructionIndex].GenerateNew();
                FocusedInstructions[FocusedInstruction] = toSet;
            }
        }


        public static void FocusVariable(int instructionIndex, int variableIndex)
        {
            if(VariableExists(instructionIndex,variableIndex))
            {
                VariableOptions.Clear();
                VariableOptions.AddRange(VariableOptionsGenerator.GetAllVariableOptions());

                FocusedInstruction = instructionIndex;
                FocusedVariable = variableIndex;
            }
        }

        public static IVariable GetVariable(int instructionIndex, int variableIndex) => FocusedInstructions[instructionIndex].Variables[variableIndex];

        public static bool FocusedVariableExists => VariableExists(FocusedInstruction, FocusedVariable);
        public static bool FocusedInstructionExists => InstructionExists(FocusedInstruction);

        public static bool VariableExists(int instructionIndex, int variableIndex)
        {
            if (InstructionExists(instructionIndex) && (FocusedInstructions[instructionIndex].VariableCount > variableIndex) && variableIndex >= 0)
            {
                return true;
            }

            return false;
        }
        public static bool InstructionExists(int instructionIndex)
        {
            if (FocusedInstructions != null && (FocusedInstructions.Count > instructionIndex) && instructionIndex>=0)
            {
                return true;
            }

            return false;

        }
        public static bool SelectableInstructionExists(int instructionIndex)
        {
            if (SelectableInstructions != null && (SelectableInstructions.Count > instructionIndex) && instructionIndex >= 0)
            {
                return true;
            }

            return false;
        }





        private static Game _game;
        public static bool IsGameWon => _game.GameWon;

        private static bool BoardRunning;
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;



        public static void InitializeDemoGame()
        {
            _game = _createDemoGame();
            
        }
        public static void InitializeEmptyGame()
        {
            _game = new Game();
        }



        public static void ManualSetBoard(Board b) => _game.SetBoard(b);

        public static void ResetBoardTemplate() => _game.ResetBoardTemplate();
        public static void ResetBoard() => _game.ResetBoard();

        public static void PauseBoard() => BoardRunning = false;
        public static void StartBoard(TimeSpan updateRate)
        {
            BoardRunning = true;
            BoardUpdateRate = updateRate;
            TimeSinceLastUpdate = TimeSpan.Zero;
        }

        public static void Update(GameTime gameTime)
        {
            if(BoardRunning)
            {
                TimeSinceLastUpdate += gameTime.ElapsedGameTime;
                if(TimeSinceLastUpdate>= BoardUpdateRate)
                {
                    TimeSinceLastUpdate = TimeSpan.Zero;
                    _game.TickBoard();
                }
            }
        }

        public static void TryMoveEntity(Entity e, Vector2Int location)
        {
            _game.CurrentBoard.TryMoveEntity(e, location);
        }

        public static Tile GetTile(int index)
        {
            return _game.CurrentBoard.TilesLinear[index];
        }

        public static void AddInstruction()
        {
            FocusedInstructions.Add(new MoveInstruction());
        }

        public static void RemoveInstruction()
        {
            if(FocusedInstructions.Any())
            {
                FocusedInstructions.RemoveAt(FocusedInstructions.Count - 1);
            }
        }



        private static Game _createDemoGame()
        {
            var game = new Game();
            var player = new EntityTemplate("Player") { Sprite = DrawUtils.PlayerSprite };

            game.SetTemplateTemplate(_createDemoTemplateTemplate(player));
            game.ResetBoardTemplate();
            game.ResetBoard();

            game.WinCondition = new GoalWinCondition(player.Id);

            FocusedInstructions = player.Instructions;

            return game;
        }

        private static BoardTemplateTemplate _createDemoTemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new BoardTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };

            player.Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Backward) };

            templateTemplate.Entities.Add(player);

            for (int i=0;i<10;i++)
            {
                templateTemplate.Entities.Add(new EntityTemplate("Wall_" + i) { Sprite = DrawUtils.WallSprite });
            }

            templateTemplate.Entities.Add(new EntityTemplate("Goal",EntityTemplate.SpecialEntityTag.Goal) { Sprite = DrawUtils.GoalSprite });

            return templateTemplate;
        }

    }
}
