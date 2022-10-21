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
    public enum CurrentFocus
    {
        None,
        Instruction,
        Variable,
        Output
    }

    internal class GameInterface
    {
        public static CurrentFocus Focus = CurrentFocus.None;

        private static int _focusedInstruction;
        private static int _focusedVariable;
        private static int _focusedOutput;

        private static List<Instruction> _focusedInstructions;
        private static List<IVariable> _variableOptions = new List<IVariable>();
        private static List<Instruction> _instructionOptions => _game?.KnownInstructions;

        public static Instruction GetInstructionFromCurrentFocus(int instructionIndex)
        {
            if(InstructionExists(instructionIndex))
            {
                return _focusedInstructions[instructionIndex];
            }

            return null;
        }
        public static Instruction GetInstructionOption(int instructionOptionIndex)
        {
            if (InstructionOptionExists(instructionOptionIndex))
            {
                return _instructionOptions[instructionOptionIndex];
            }

            return null;
        }
        public static IVariable GetVariableOption(int variableOptionIndex)
        {
            if (VariableOptionExists(variableOptionIndex))
            {
                return _variableOptions[variableOptionIndex];
            }

            return null;
        }

        public static bool IsFocusedOnInstruction(int instructionIndex) => instructionIndex == _focusedInstruction;
        public static bool IsFocusedOnVariable(int instructionIndex,int variableIndex) => instructionIndex == _focusedInstruction & variableIndex == _focusedVariable & Focus == CurrentFocus.Variable;
        public static bool IsFocusedOnOutput(int instructionIndex, int outputIndex) => instructionIndex == _focusedInstruction & outputIndex == _focusedOutput & Focus == CurrentFocus.Output;

        public static void AssignValueToFocusedVariable(int variableOptionIndex)
        {
            if(FocusedVariableExists && VariableOptionExists(variableOptionIndex))
            {
                var newVariable = _variableOptions[variableOptionIndex];
                _focusedInstructions[_focusedInstruction].Variables[_focusedVariable] = newVariable;
            }
        }
        public static void AssignValueToFocusedInstruction(int instructionOptionIndex)
        {
            if(FocusedInstructionExists && InstructionOptionExists(instructionOptionIndex))
            {
                var toSet = _instructionOptions[instructionOptionIndex].GenerateNew();
                _focusedInstructions[_focusedInstruction] = toSet;
            }
        }
        public static void AssignValueToFocusedOutput(int outputOptionIndex)
        {
            if (FocusedOutputExists && OutputOptionExists(outputOptionIndex))
            {
                GetInstructionFromCurrentFocus(_focusedInstruction).OutputTargets[_focusedOutput] = outputOptionIndex;
            }
        }

        public static void FocusInstruction(int instructionIndex)
        {
            if (InstructionExists(instructionIndex))
            {
                _focusedInstruction = instructionIndex;
                Focus = CurrentFocus.Instruction;
            }
        }
        public static void FocusVariable(int instructionIndex, int variableIndex)
        {
            if(VariableExists(instructionIndex,variableIndex))
            {
                _variableOptions.Clear();
                _variableOptions.AddRange(VariableOptionsGenerator.GetAllVariableOptions());

                _focusedInstruction = instructionIndex;
                _focusedVariable = variableIndex;
                Focus = CurrentFocus.Variable;
            }
        }
        public static void FocusOutput(int instructionIndex, int outputIndex)
        {
            if (OutputExists(instructionIndex, outputIndex))
            {
                //VariableOptions.Clear();
                //VariableOptions.AddRange(VariableOptionsGenerator.GetAllVariableOptions());

                _focusedInstruction = instructionIndex;
                _focusedOutput = outputIndex;
                Focus = CurrentFocus.Output;
            }
        }

        public static IVariable GetVariable(int instructionIndex, int variableIndex) => _focusedInstructions[instructionIndex].Variables[variableIndex];

        public static bool FocusedOutputExists => OutputExists(_focusedInstruction, _focusedOutput);
        public static bool FocusedVariableExists => VariableExists(_focusedInstruction, _focusedVariable);
        public static bool FocusedInstructionExists => InstructionExists(_focusedInstruction);

        public static bool OutputExists(int instructionIndex, int outputIndex)
        {
            if (InstructionExists(instructionIndex) && outputIndex >= 0 && (_focusedInstructions[instructionIndex].OutputCount > outputIndex))
            {
                return true;
            }

            return false;
        }
        public static bool VariableExists(int instructionIndex, int variableIndex)
        {
            if (InstructionExists(instructionIndex) && variableIndex >= 0 && (_focusedInstructions[instructionIndex].VariableCount > variableIndex))
            {
                return true;
            }

            return false;
        }
        public static bool InstructionExists(int instructionIndex)
        {
            return (_focusedInstructions != null && (_focusedInstructions.Count > instructionIndex) && instructionIndex >= 0);
        }
        
        public static bool InstructionOptionExists(int instructionIndex)
        {
            return (_game != null && _instructionOptions != null && (_instructionOptions.Count > instructionIndex) && instructionIndex >= 0);

        }
        public static bool VariableOptionExists(int variableOptionIndex)
        {
            return (_variableOptions != null && (_variableOptions.Count > variableOptionIndex) && variableOptionIndex >= 0);
        }
        public static bool OutputOptionExists(int outputOptionIndex)
        {
            return ((Config.EntityMaxVariables > outputOptionIndex) && outputOptionIndex >= 0);
        }






        private static Game _game;
        public static bool IsGameWon => _game.GameWon;

        private static bool BoardRunning;
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;


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
            _focusedInstructions.Add(new MoveInstruction());
        }

        public static void RemoveInstruction()
        {
            if(_focusedInstructions.Any())
            {
                _focusedInstructions.RemoveAt(_focusedInstructions.Count - 1);
            }
        }





    }
}
