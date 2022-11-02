﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2
{

    public partial class GameInterface
    {
        public static void AddInstructionToEnd() => AddInstructionAtIndex(_focusedInstructions.Count);
        public static void RemoveInstructionFromEnd() => RemoveInstructionAtIndex(_focusedInstructions.Count-1);
        public static void AddInstructionAtIndex(int index)
        {
            if (index < 0 || index > _focusedInstructions.Count)
            {
                return;
            }

            _focusedInstructions.Insert(index, InstructionDatabase.GenerateRandom());
        }
        public static void RemoveInstructionAtIndex(int index)
        {
            if(InstructionExists(index))
            {
                _focusedInstructions.RemoveAt(index);
            }
        }

        public static CurrentFocus Focus = CurrentFocus.Instruction;

        private static int _focusedInstruction;
        private static int _focusedVariable;
        private static int _focusedOutput;
        private static int _focusedControlOutput;
        private static int _focusedInstructionOption;
        private static int _focusedVariableOption;

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
        public static IVariable GetVariable(int instructionIndex, int variableIndex) => _focusedInstructions[instructionIndex].Variables[variableIndex];

        public static bool IsFocusedOnInstruction(int instructionIndex) => instructionIndex == _focusedInstruction;
        public static bool IsFocusedOnInstructionOption(int optionIndex) =>  optionIndex == _focusedInstructionOption & Focus == CurrentFocus.InstructionOption;
        public static bool IsFocusedOnVariable(int instructionIndex,int variableIndex) => instructionIndex == _focusedInstruction & variableIndex == _focusedVariable & Focus == CurrentFocus.Variable;
        public static bool IsFocusedOnVariableOption(int optionIndex) => optionIndex == _focusedVariableOption & Focus == CurrentFocus.VariableOption;

        public static bool IsFocusedOnOutput(int instructionIndex, int outputIndex) => instructionIndex == _focusedInstruction & outputIndex == _focusedOutput & Focus == CurrentFocus.Output;
        public static bool IsFocusedOnControlOutput(int instructionIndex, int controlIndex) => instructionIndex == _focusedInstruction & controlIndex == _focusedControlOutput & Focus == CurrentFocus.ControlOutput;

        public static void AssignValueToFocusedVariable(int variableOptionIndex)
        {
            if((Focus == CurrentFocus.Variable || Focus == CurrentFocus.VariableOption) && FocusedVariableExists && VariableOptionExists(variableOptionIndex))
            {
                var newVariable = _variableOptions[variableOptionIndex];
                _focusedInstructions[_focusedInstruction].Variables[_focusedVariable] = newVariable;
            }
        }
        public static void AssignValueToFocusedControlOutput(int targetIndex)
        {
            if (Focus == CurrentFocus.ControlOutput && FocusedControlOutputExists)
            {
                _focusedInstructions[_focusedInstruction].ControlOutputs[_focusedControlOutput] = targetIndex;
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
            if (Focus == CurrentFocus.Output && FocusedOutputExists && OutputOptionExists(outputOptionIndex))
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
                _focusedInstruction = instructionIndex;
                _focusedOutput = outputIndex;
                Focus = CurrentFocus.Output;
            }
        }
        public static void FocusControlOutput(int instructionIndex, int controlIndex)
        {
            if (ControlOutputExists(instructionIndex, controlIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedControlOutput = controlIndex;
                Focus = CurrentFocus.ControlOutput;
            }
        }
        public static void FocusInstructionOption(int instructionIndex, int optionIndex)
        {
            if(InstructionExists(instructionIndex) &&  InstructionOptionExists(optionIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedInstructionOption = optionIndex;
                Focus = CurrentFocus.InstructionOption;
            }
        }
        public static void FocusVariableOption(int instructionIndex, int variableIndex,int variableOptionIndex)
        {
            if (InstructionExists(instructionIndex) && VariableExists(instructionIndex,variableIndex) && VariableOptionExists(variableOptionIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedVariable = variableIndex;
                _focusedVariableOption = variableOptionIndex;
                Focus = CurrentFocus.VariableOption;
            }
        }

        public static bool FocusedOutputExists => OutputExists(_focusedInstruction, _focusedOutput);
        public static bool FocusedVariableExists => VariableExists(_focusedInstruction, _focusedVariable);
        public static bool FocusedControlOutputExists => ControlOutputExists(_focusedInstruction, _focusedControlOutput);
        public static bool FocusedInstructionExists => InstructionExists(_focusedInstruction);

        public static bool OutputExists(int instructionIndex, int outputIndex)
        {
            if (InstructionExists(instructionIndex) && outputIndex >= 0 && (_focusedInstructions[instructionIndex].OutputCount > outputIndex))
            {
                return true;
            }

            return false;
        }
        public static bool ControlOutputExists(int instructionIndex, int controlIndex)
        {
            if (InstructionExists(instructionIndex) && controlIndex >= 0 && (_focusedInstructions[instructionIndex].ControlOutputCount > controlIndex))
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
        public static bool InstructionOptionExists(int instructionOptionIndex)
        {
            return (_game != null && _instructionOptions != null && (_instructionOptions.Count > instructionOptionIndex) && instructionOptionIndex >= 0);

        }
        public static bool VariableOptionExists(int variableOptionIndex)
        {
            return (_variableOptions != null && (_variableOptions.Count > variableOptionIndex) && variableOptionIndex >= 0);
        }
        public static bool OutputOptionExists(int outputOptionIndex)
        {
            return ((Config.EntityMaxVariables > outputOptionIndex) && outputOptionIndex >= 0);
        }
    }
}