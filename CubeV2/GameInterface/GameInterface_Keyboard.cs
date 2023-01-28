﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        private static void _processKeyboardActions(UserInput input)
        {
            if (input.IsKeyJustPressed(Keys.Space))
            {
                TogglePauseBoard();
            }

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

                    FocusInstruction(_focusedInstruction+1);
                }
                else if (input.IsKeyJustPressed(Keys.Right))
                {
                    FocusInstructionOption(_focusedInstruction, 0);
                }
                else if (input.IsKeyJustPressed(Keys.Enter))
                {   
                    FocusVariable(_focusedInstruction, 0);

                    if (FocusedInstructionExists & _focusedInstructions[_focusedInstruction].VariableCount == 1)
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
            else if (CurrentSidePanelFocus == SidePanelFocus.Variable)
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
                else if(input.IsKeyJustPressed(Keys.Up))
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
                if (input.IsKeyJustPressed(Keys.Enter))
                {
                    AssignValueToFocusedVariable(_focusedVariableOption);
                    FocusVariable(_focusedInstruction,_focusedVariable);
                }
                else if (input.IsKeyJustPressed(Keys.Left))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption - 1);
                }
                else if (input.IsKeyJustPressed(Keys.Right))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption + 1);
                }
                else if (input.IsKeyJustPressed(Keys.Down))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption + Config.VariableSelectorGridSize.X);
                }
                else if (input.IsKeyJustPressed(Keys.Up))
                {
                    FocusVariableOption(_focusedInstruction, _focusedVariable, _focusedVariableOption - Config.VariableSelectorGridSize.X);
                }

            }

            if (input.IsKeyDown(Keys.Left))
            {
                CameraOffset.X++;
            }
             if (input.IsKeyDown(Keys.Right))
            {
                CameraOffset.X--;
            }
             if (input.IsKeyDown(Keys.Down))
            {
                CameraOffset.Y--;
            }
             if (input.IsKeyDown(Keys.Up))
            {
                CameraOffset.Y++;
            }
            if (input.IsKeyJustReleased(Keys.OemPlus))
            {
                SetCameraConfig(CameraScale + 1);
            }
            if (input.IsKeyJustReleased(Keys.OemMinus))
            {
                SetCameraConfig(CameraScale - 1);
            }


        }
    }
}