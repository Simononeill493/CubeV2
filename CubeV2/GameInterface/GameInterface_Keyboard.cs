using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        private static void _processEditorKeyboardShortcuts(UserInput input)
        {
            if (CurrentSidePanelFocus == SidePanelFocus.Instruction)
            {
                if (input.IsKeyJustPressed(Keys.Up))
                {
                    FocusInstruction(_focusedInstruction - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Down))
                {
                    if (input.CtrlDown)
                    {
                        AddInstructionAtIndex(_focusedInstruction);
                    }

                    FocusInstruction(_focusedInstruction + 1);
                }
                else if (input.IsKeyJustPressed(Keys.Right))
                {
                    FocusInstructionOption(_focusedInstruction, 0);
                }
                else if (input.IsKeyJustPressed(Keys.Enter))
                {
                    FocusVariable(_focusedInstruction, 0);

                    if (FocusedInstructionExists && _focusedInstructions[FocusedInstructionSet][_focusedInstruction].VariableCount == 1)
                    {
                        FocusVariableOption(_focusedInstruction, 0, 0);
                    }
                }

            }
            else if (CurrentSidePanelFocus == SidePanelFocus.InstructionOption)
            {
                if (input.IsKeyJustPressed(Keys.Up))
                {
                    FocusInstructionOption(_focusedInstruction, _focusedInstructionOption - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Down))
                {
                    FocusInstructionOption(_focusedInstruction, _focusedInstructionOption + 1);
                }
                else if (input.IsKeyJustPressed(Keys.Left))
                {
                    FocusInstruction(_focusedInstruction);
                }
                else if (input.IsKeyJustPressed(Keys.Enter) || input.IsKeyJustPressed(Keys.Right))
                {
                    AssignValueToFocusedInstruction(_focusedInstructionOption);
                    FocusInstruction(_focusedInstruction);
                }
            }
            else if (CurrentSidePanelFocus == SidePanelFocus.VariableCategory)
            {
                if (input.IsKeyJustPressed(Keys.Enter))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, 0);
                }
                else if (input.IsKeyJustPressed(Keys.Left))
                {
                    FocusVariable(_focusedInstruction, _focusedVariable + 1);
                }
                else if (input.IsKeyJustPressed(Keys.Right))
                {
                    FocusVariable(_focusedInstruction, _focusedVariable - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Up))
                {
                    FocusInstruction(_focusedInstruction - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Down))
                {
                    if (input.CtrlDown)
                    {
                        AddInstructionAtIndex(_focusedInstruction);
                    }

                    FocusInstruction(_focusedInstruction + 1);
                }
            }
            else if (CurrentSidePanelFocus == SidePanelFocus.VariableOption)
            {
                if (input.IsKeyJustPressed(Keys.Left))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Right))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption + 1);
                }
                else if (input.IsKeyJustPressed(Keys.Down))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption + Config.VariableCategoryListSize.X);
                }
                else if (input.IsKeyJustPressed(Keys.Up))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption - Config.VariableCategoryListSize.X);
                }

            }
        }
        
        private static void _processBoardKeyInput(UserInput input)
        {
            if (input.IsKeyJustPressed(Keys.Space))
            {
                TogglePauseBoard();
            }

            var arrowsDirection = DirectionUtils.GetArrowsDirection(input);
            if (arrowsDirection.AnyPressed && !Config.LockCameraMovement)
            {
                CameraOffset += arrowsDirection.Direction.ToVector();
            }

            if(!Config.LockZoomLevel)
            {
                if (input.IsKeyJustReleased(Keys.OemPlus) | input.IsButtonJustReleased(Buttons.RightShoulder))
                {
                    SetCameraConfig(CameraScale + 1);
                    CenterCameraOnPlayer();
                }
                if (input.IsKeyJustReleased(Keys.OemMinus) | input.IsButtonJustReleased(Buttons.LeftShoulder))
                {
                    SetCameraConfig(CameraScale - 1);
                    CenterCameraOnPlayer();
                }
            }
            if (input.IsKeyJustReleased(Keys.Enter))
            {
                CenterCameraOnPlayer();
            }
        }
        
        
        private static void _processKeyboardActions(UserInput input)
        {
            switch (PrimaryFocus)
            {
                case PrimaryFocus.Board:
                    _processBoardKeyInput(input);
                    break;
                case PrimaryFocus.Editor:
                    _processEditorKeyboardShortcuts(input);
                    break;
            }
        }
    }
}