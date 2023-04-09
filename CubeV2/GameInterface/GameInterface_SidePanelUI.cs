using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static SidePanelFocus CurrentSidePanelFocus = SidePanelFocus.Instruction;

        private static int _focusedInstruction;
        private static int _focusedVariable;
        private static int _focusedOutput;
        private static int _focusedControlOutput;
        private static int _focusedInstructionOption;
        private static int _focusedVariableOption;
        private static int _focusedTile;
        public static int FocusedInstructionSet { get; private set; } = 0;

        private static EntityTemplate _focusedTemplate;
        private static List<Instruction[]> _focusedInstructions => _focusedTemplate.Instructions;
        private static List<Instruction> _instructionOptions => _game?.KnownInstructions;
        private static List<IVariable> _focusedGenericVariables;

        public static void AddInstructionSet()
        {
            _focusedInstructions.Add(new Instruction[Config.EntityMaxInstructionsPerSet]);
        }
        public static void AddInstructionAtIndex(int index)
        {
            if (index < 0 || index > Config.EntityMaxInstructionsPerSet )
            {
                return;
            }

            _focusedInstructions[FocusedInstructionSet][index] = new MoveInstruction();
        }
        public static void RemoveInstructionAtIndex(int index)
        {
            if(InstructionExists(index))
            {
                _focusedInstructions[FocusedInstructionSet][index] = null;
            }
        }

        public static Instruction GetInstructionFromCurrentFocus(int instructionIndex)
        {
            if(InstructionSetExists(FocusedInstructionSet) && InstructionExists(instructionIndex))
            {
                return _focusedInstructions[FocusedInstructionSet][instructionIndex];
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
        public static VariableCategory GetVariableCategory(int categoryIndex)
        {
            var categories = VariableOptionsGenerator.GetAllVariableCategories();
            if (categoryIndex < categories.Count)
            {
                return categories[categoryIndex];
            }

            return null;
        }
        public static IVariable GetInstructionVariable(int instructionIndex, int variableIndex) => _focusedInstructions[FocusedInstructionSet][instructionIndex].Variables[variableIndex];
        public static IVariable GetVariableFromGrid(int variableIndex)
        {
            if (variableIndex >= 0 && variableIndex < _focusedGenericVariables.Count)
            {
                return _focusedGenericVariables[variableIndex];
            }

            return null;
        }

        public static bool IsFocusedOnInstruction(int instructionIndex) => instructionIndex == _focusedInstruction;
        public static bool IsFocusedOnInstructionOption(int optionIndex) =>  optionIndex == _focusedInstructionOption & CurrentSidePanelFocus == SidePanelFocus.InstructionOption;
        public static bool IsFocusedOnVariable(int instructionIndex,int variableIndex) => instructionIndex == _focusedInstruction & variableIndex == _focusedVariable & CurrentSidePanelFocus == SidePanelFocus.VariableCategory;
        public static bool IsFocusedOnVariableOption(int optionIndex) => optionIndex == _focusedVariableOption & CurrentSidePanelFocus == SidePanelFocus.VariableOption;

        public static bool IsFocusedOnOutput(int instructionIndex, int outputIndex) => instructionIndex == _focusedInstruction & outputIndex == _focusedOutput & CurrentSidePanelFocus == SidePanelFocus.Output;
        public static bool IsFocusedOnControlOutput(int instructionIndex, int controlIndex) => instructionIndex == _focusedInstruction & controlIndex == _focusedControlOutput & CurrentSidePanelFocus == SidePanelFocus.ControlOutput;

        public static void VaribleCategorySelected(int categoryIndex)
        {
            if(VariableCategoryExists(categoryIndex))
            {
                var category = VariableUtils.GetAllVariableCategories()[categoryIndex];
                if(category.Name == VariableUtils.IntegerVariableName)
                {
                    CurrentSidePanelFocus = SidePanelFocus.IntegerVariableMaker;
                }
                else
                {
                    _focusedGenericVariables = VariableOptionsGenerator.GetVariableOptions(VariableUtils.GetAllVariableCategories()[categoryIndex]);
                    CurrentSidePanelFocus = SidePanelFocus.VariableGeneric;
                }
            }
        }

        public static void AssignValueToFocusedVariableFromGrid(int variableGridIndex) => AssignValueToFocusedVariable(GetVariableFromGrid(variableGridIndex));
        public static void AssignValueToFocusedVariable(IVariable variable)
        {
            if (variable != null)
            {
                if (FocusedVariableExists && FocusedInstructionExists)
                {
                    _focusedInstructions[FocusedInstructionSet][_focusedInstruction].Variables[_focusedVariable] = variable;
                }
            }
        }

        public static void TryAssignIntegerValueToFocusedVariable(string integerTextBoxString)
        {
            int result;
            if(int.TryParse(integerTextBoxString, out result))
            {
                _focusedInstructions[FocusedInstructionSet][_focusedInstruction].Variables[_focusedVariable] = new IntegerVariable(result);
            }
        }




        public static void AssignValueToFocusedControlOutput(int targetIndex)
        {
            if (CurrentSidePanelFocus == SidePanelFocus.ControlOutput && FocusedControlOutputExists)
            {
                _focusedInstructions[FocusedInstructionSet][_focusedInstruction].ControlFlowOutputs[_focusedControlOutput] = targetIndex;
            }
        }
        public static void AssignValueToFocusedInstruction(int instructionOptionIndex)
        {
            if(InstructionOptionExists(instructionOptionIndex))
            {
                if(!FocusedInstructionExists)
                {
                    AddInstructionAtIndex(_focusedInstruction);
                }

                var toSet = _instructionOptions[instructionOptionIndex].GenerateNew();
                _focusedInstructions[FocusedInstructionSet][_focusedInstruction] = toSet;

            }
        }
        public static void AssignValueToFocusedOutput(int outputOptionIndex)
        {
            if (CurrentSidePanelFocus == SidePanelFocus.Output && FocusedOutputExists && OutputOptionExists(outputOptionIndex))
            {
                GetInstructionFromCurrentFocus(_focusedInstruction).OutputTargetVariables[_focusedOutput] = outputOptionIndex;
            }
        }

        public static void FocusTile(int tileIndex)
        {
            if(TileExists(tileIndex))
            {
                _focusedTile = tileIndex;
                CurrentSidePanelFocus = SidePanelFocus.Tile;
            }
        }
        public static void FocusInstructionSet(int index)
        {
            if(InstructionSetExists(index))
            {
                FocusedInstructionSet = index;
            }
        }
        public static void FocusInstruction(int instructionIndex)
        {
            //if (InstructionExists(instructionIndex))
            //{
                _focusedInstruction = instructionIndex;
                CurrentSidePanelFocus = SidePanelFocus.Instruction;
            //}
        }
        public static void FocusVariable(int instructionIndex, int variableIndex)
        {
            if(VariableExists(instructionIndex,variableIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedVariable = variableIndex;
                CurrentSidePanelFocus = SidePanelFocus.VariableCategory;
            }
        }
        public static void FocusOutput(int instructionIndex, int outputIndex)
        {
            if (OutputExists(instructionIndex, outputIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedOutput = outputIndex;
                CurrentSidePanelFocus = SidePanelFocus.Output;
            }
        }
        public static void FocusControlOutput(int instructionIndex, int controlIndex)
        {
            if (ControlOutputExists(instructionIndex, controlIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedControlOutput = controlIndex;
                CurrentSidePanelFocus = SidePanelFocus.ControlOutput;
            }
        }
        public static void FocusInstructionOption(int instructionIndex, int optionIndex)
        {
            if(InstructionOptionExists(optionIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedInstructionOption = optionIndex;
                CurrentSidePanelFocus = SidePanelFocus.InstructionOption;
            }
        }
        public static void FocusVariableOption(int instructionIndex, int variableIndex,int variableOptionIndex)
        {
            throw new NotImplementedException();
            /*if (InstructionExists(instructionIndex) && VariableExists(instructionIndex,variableIndex) && VariableOptionExists(variableOptionIndex))
            {
                _focusedInstruction = instructionIndex;
                _focusedVariable = variableIndex;
                _focusedVariableOption = variableOptionIndex;
                CurrentSidePanelFocus = SidePanelFocus.VariableOption;
            }*/
        }

        public static bool FocusedOutputExists => OutputExists(_focusedInstruction, _focusedOutput);
        public static bool FocusedVariableExists => VariableExists(_focusedInstruction, _focusedVariable);
        public static bool FocusedControlOutputExists => ControlOutputExists(_focusedInstruction, _focusedControlOutput);
        public static bool FocusedInstructionExists => InstructionExists(_focusedInstruction);
        public static bool FocusedTileExists => TileExists(_focusedTile);

        public static bool OutputExists(int instructionIndex, int outputIndex)
        {
            if (InstructionExists(instructionIndex) && outputIndex >= 0 && (_focusedInstructions[FocusedInstructionSet][instructionIndex].OutputCount > outputIndex))
            {
                return true;
            }

            return false;
        }
        public static bool ControlOutputExists(int instructionIndex, int controlIndex)
        {
            if (InstructionExists(instructionIndex) && controlIndex >= 0 && (_focusedInstructions[FocusedInstructionSet][instructionIndex].ControlOutputCount > controlIndex))
            {
                return true;
            }

            return false;
        }
        public static bool VariableExists(int instructionIndex, int variableIndex)
        {
            if (InstructionExists(instructionIndex) && variableIndex >= 0 && (_focusedInstructions[FocusedInstructionSet][instructionIndex].VariableCount > variableIndex))
            {
                return true;
            }

            return false;
        }
        public static bool VariableCategoryExists(int categoryIndex)
        {
            if (categoryIndex >= 0 && categoryIndex < VariableUtils.GetAllVariableCategories().Count)
            {
                return true;
            }

            return false;
        }

        public static bool InstructionSetExists(int instructionSetIndex)
        {
            return (_focusedInstructions != null && instructionSetIndex >= 0 && instructionSetIndex < _focusedInstructions.Count );
        }

        public static bool InstructionExists(int instructionIndex)
        {
            return (_focusedInstructions != null && (Config.EntityMaxInstructionsPerSet > instructionIndex) && instructionIndex >= 0 && InstructionSetExists(FocusedInstructionSet) && _focusedInstructions[FocusedInstructionSet]?[instructionIndex]!=null);
        }
        public static bool InstructionOptionExists(int instructionOptionIndex)
        {
            return (_game != null && _instructionOptions != null && (_instructionOptions.Count > instructionOptionIndex) && instructionOptionIndex >= 0);

        }
        public static bool VariableOptionExists(int variableOptionIndex)
        {
            throw new NotImplementedException();
           // return (_variableCategories != null && (_variableCategories.Count > variableOptionIndex) && variableOptionIndex >= 0);
        }
        public static bool OutputOptionExists(int outputOptionIndex)
        {
            return ((Config.EntityMaxVariables > outputOptionIndex) && outputOptionIndex >= 0);
        }
    }
}
