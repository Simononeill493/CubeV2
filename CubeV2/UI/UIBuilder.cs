using CubeV2.UI.Appearance.OutputSelectionTile;
using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace CubeV2
{
    internal class UIBuilder
    {
        public static UIElement GenerateUI()
        {
            var topLevel = new UIElement(Config.UITopLevelName);

            var instructionPanel = UIElementMaker.MakeRectangle(Config.InstructionPanelName, Config.InstructionPanelSize, Config.InstructionPanelOffset,Config.InstructionPanelColor, DrawUtils.UILayer1);
            
            var selectorPanel = UIElementMaker.MakeRectangle(Config.SelectorPanelName, Config.SelectorPanelSize, Config.SelectorPanelOffset, Config.SelectorPanelColor, DrawUtils.UILayer1);

            var gameGrid = _makeGameGrid();
            gameGrid.SetOffset(Config.InstructionPanelSize.X + Config.SelectorPanelSize.X, 0);

            var rerollButton = UIElementMaker.MakeRectangle(Config.RerollButtonName, Config.GameControlButtonSize, Config.RerollButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            rerollButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reroll"));
            rerollButton.AddLeftClickAction((i) => { GameInterface.ResetBoardTemplate(); GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var goButton = UIElementMaker.MakeRectangle(Config.GoButtonName, Config.GameControlButtonSize, Config.GoButtonOffset,Color.AliceBlue, DrawUtils.UILayer2);
            goButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Go"));
            goButton.AddLeftClickAction((i) => GameInterface.StartBoard(Config.DemoBoardUpdateRate));

            var resetButton = UIElementMaker.MakeRectangle(Config.ResetButtonName, Config.GameControlButtonSize, Config.ResetButtonOffset,Color.AliceBlue, DrawUtils.UILayer2);
            resetButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reset"));
            resetButton.AddLeftClickAction((i) => { GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var winText = new UIElement(Config.WinTextName);
            winText.SetOffset(Config.WinTextOffset);
            winText.AddAppearance(new TextAppearance(new Color(255,58,200), DrawUtils.UILayer1, "A winner is you!"));
            winText.SetEnabledCondition(() => GameInterface.IsGameWon);


            topLevel.AddChildren(instructionPanel, selectorPanel, gameGrid,rerollButton,goButton,resetButton,winText);





            var instructionTiles = _makeInstructionTiles();

            var addInstructionButton = UIElementMaker.MakeRectangle(Config.AddInstructionButtonName, new Vector2(50,30), Config.AddInstructionButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            addInstructionButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3,"+"));
            addInstructionButton.AddLeftClickAction((i) => GameInterface.AddInstruction());

            var removeInstructionButton = UIElementMaker.MakeRectangle(Config.RemoveInstructionButtonName, new Vector2(50, 30), Config.RemoveInstructionButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            removeInstructionButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3,"-"));
            removeInstructionButton.AddLeftClickAction((i) => GameInterface.RemoveInstruction());

            instructionPanel.AddChildren(addInstructionButton, removeInstructionButton, instructionTiles);

            var instructionSelectorGrid = _makeInstructionSelectorGrid();
            instructionSelectorGrid.SetOffset(20, 20);
            instructionSelectorGrid.SetEnabledCondition(InstructionSelectorGridEnabled);

            var variableSelectorGrid = _makeVariableSelectorGrid();
            variableSelectorGrid.SetOffset(20, 20);
            variableSelectorGrid.SetEnabledCondition(VariableSelectorGridEnabled);

            var outputSelectorGrid = _makeOutputSelectorGrid();
            outputSelectorGrid.SetOffset(20, 20);
            outputSelectorGrid.SetEnabledCondition(OutputSelectorGridEnabled);

            selectorPanel.AddChildren(instructionSelectorGrid,variableSelectorGrid,outputSelectorGrid);

            return topLevel;
        }

        private static bool InstructionSelectorGridEnabled()
        {
            return GameInterface.Focus == CurrentFocus.Instruction && GameInterface.FocusedInstructionExists;
        }

        private static bool VariableSelectorGridEnabled()
        {
            return GameInterface.Focus == CurrentFocus.Variable && GameInterface.FocusedVariableExists;
        }

        private static bool OutputSelectorGridEnabled()
        {
            return GameInterface.Focus == CurrentFocus.Output && GameInterface.FocusedOutputExists;
        }

        private static UIGrid _makeGameGrid()
        {
            var gameTileSize = Config.TileBaseSize * Config.TileScale;
            var gameGrid = UIGrid.Make(Config.GameGridName,Config.GameGridWidth,Config.GameGridHeight, (int)gameTileSize.X,(int)gameTileSize.Y,Config.GameGridPadding,Config.TileBackgroundColor,DrawUtils.UILayer1,new GameTileAppearanceFactory(DrawUtils.UILayer2,DrawUtils.UILayer3));

            return gameGrid;
        }

        private static UIGrid _makeVariableSelectorGrid()
        {
            var gridSize = new Vector2Int(4, 7);
            var tileSize = Config.TileBaseSize * Config.VariableSelectionTileScale;
            var tilePadding = 3;
            var backgroundColor = Config.SelectionTileVariableColor;
            var backgroundDrawLayer = DrawUtils.UILayer2;
            var appearanceDrawLayer = DrawUtils.UILayer3;
            var appearanceFactory = new VariableTileAppearanceFactory_ForSelectionGrid(Config.VariableSelectionTileScale, appearanceDrawLayer);

            var variableGrid = UIGrid.Make(Config.VariableGridName, gridSize, new Vector2Int(tileSize), tilePadding, backgroundColor, backgroundDrawLayer, appearanceFactory);
            variableGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedVariable(index);
            return variableGrid;
        }

        private static UIGrid _makeInstructionSelectorGrid()
        {
            var gridSize = new Vector2Int(1, 9);
            var tileSize = new Vector2Int(Config.InstructionOptionTileSize);
            var tilePadding = 5;

            var backgroundColor = Config.InstructionSelectorTileColor;
            var backgroundDrawLayer = DrawUtils.UILayer2;
            var appearanceDrawLayer = DrawUtils.UILayer3;
            var appearanceFactory = new InstructionSelectionTileAppearanceFactory(appearanceDrawLayer);

            var instructionGrid = UIGrid.Make(Config.InstructionSelectorGridName, gridSize, tileSize, tilePadding, backgroundColor, backgroundDrawLayer, appearanceFactory);
            instructionGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedInstruction(index);
            return instructionGrid;
        }

        private static UIGrid _makeOutputSelectorGrid()
        {
            var gridSize = new Vector2Int(1, Config.EntityMaxVariables);
            var tileSize = new Vector2Int(Config.InstructionOptionTileSize);
            var tilePadding = 10;

            var backgroundColor = Config.InstructionSelectorTileColor;
            var backgroundDrawLayer = DrawUtils.UILayer2;
            var appearanceDrawLayer = DrawUtils.UILayer3;
            var appearanceFactory = new OutputSelectionTileAppearanceFactory(Config.VariableSelectionTileScale,appearanceDrawLayer);

            var outputSelectorGrid = UIGrid.Make(Config.OutputSelectorGridName, gridSize, tileSize, tilePadding, backgroundColor, backgroundDrawLayer, appearanceFactory);
            outputSelectorGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedOutput(index);
            return outputSelectorGrid;
        }



        private static UIElement _makeInstructionTiles()
        {
            var slotContainer = new UIElement("SlotContainer");
            slotContainer.SetOffset(Config.InstructionPanelSize.X / 2 - Config.InstructionTileSize.X / 2, Config.InstructionTileTopPadding);

            for(int i=0;i<Config.NumInstructionTiles;i++)
            {
                var slot = _makeInstructionTile(i);
                slotContainer.AddChildren(slot);
            }

            return slotContainer;
        }

        private static UIElement _makeInstructionTile(int instructionIndex)
        {
            var slot = new UIElement(Config.InstructionSlotName + '_' + instructionIndex);

            var slotBackground = new RectangleAppearance(Config.InstructionTileSize, Config.InstructionTileColor, DrawUtils.UILayer2);
            slotBackground.OverrideColor(() => (GameInterface.IsFocusedOnInstruction(instructionIndex) ? Config.InstructionTileHighlightColor : Config.InstructionTileColor));
            slot.AddAppearances(slotBackground, new InstructionTileAppearance(instructionIndex,DrawUtils.UILayer3));
            slot.SetOffset(0, (Config.InstructionTileInternalPadding + Config.InstructionTileSize.Y) * instructionIndex);

            slot.AddLeftClickAction((i) => { GameInterface.FocusInstruction(instructionIndex); });


            for (int variableIndex = 0; variableIndex < Config.InstructionMaxNumVariables; variableIndex++)
            {
                var variableTile = _makeInstructionVariableTile(instructionIndex, variableIndex);
                slot.AddChildren(variableTile);
            }

            for(int outputIndex = 0;outputIndex < Config.InstructionMaxNumOutputs;outputIndex++)
            {
                var outputTile = _makeInstructionOutputTile(instructionIndex,outputIndex);
                slot.AddChildren(outputTile);
            }

            for (int controlIndex = 0; controlIndex < Config.InstructionMaxNumControlOutputs; controlIndex++)
            {
                var controlOutputTile = _makeInstructionControlTile(instructionIndex, controlIndex);
                slot.AddChildren(controlOutputTile);
            }



            var indexText = new UIElement(slot.ID + "_indexNumber");
            indexText.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer2, instructionIndex.ToString()));
            indexText.SetOffset(-20, Config.InstructionTileSize.Y / 2);
            slot.AddChildren(indexText);

            return slot;
        }

        private static UIElement _makeInstructionOutputTile(int instructionIndex, int outputIndex)
        {
            var outputTile = new UIElement(Config.InstructionOutputTileName + "_" + instructionIndex + "_" + outputIndex);
            outputTile.SetOffset(Config.InstructionTileSize.X+1,(outputIndex*50));

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileColor, DrawUtils.UILayer2);
            tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnOutput(instructionIndex,outputIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));
            var tileAppearance = new OutputTileAppearance(outputIndex, instructionIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer3);
            outputTile.AddAppearances(tileBackground,tileAppearance);

            outputTile.AddLeftClickAction((i) => { GameInterface.FocusOutput(instructionIndex, outputIndex); });
            outputTile.SetEnabledCondition(() => GameInterface.OutputExists(instructionIndex, outputIndex));

            return outputTile;
        }

        private static UIElement _makeInstructionControlTile(int instructionIndex, int controlIndex)
        {
            var outputTile = new UIElement(Config.InstructionControlOutputTileName + "_" + instructionIndex + "_" + controlIndex);
            outputTile.SetOffset(Config.InstructionTileSize.X + 1, (controlIndex * 50));

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileColor, DrawUtils.UILayer2);
            tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnControlOutput(instructionIndex, controlIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));
            var tileAppearance = new OutputControlTileAppearance(controlIndex, instructionIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer3);
            outputTile.AddAppearances(tileBackground, tileAppearance);

            outputTile.AddLeftClickAction((i) => { GameInterface.FocusControlOutput(instructionIndex, controlIndex); });
            outputTile.SetEnabledCondition(() => GameInterface.ControlOutputExists(instructionIndex, controlIndex));
            outputTile.AddKeyPressedAction((i) => { if (i.IsNumberJustPressed) { GameInterface.AssignValueToFocusedControlOutput(i.GetNumberJustPressed); } });

            return outputTile;
        }


        private static UIElement _makeInstructionVariableTile(int instructionIndex,int variableIndex)
        {
            var variableTile = new UIElement(Config.InstructionVariableTileName + "_" + instructionIndex + "_" + variableIndex);
            variableTile.SetOffset((Config.TileBaseSize.X * Config.InstructionTileVariableScale + 20) * variableIndex, 20);

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileAssignedVariableColor, DrawUtils.UILayer3);
            tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnVariable(instructionIndex,variableIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));

            var appearance = new VariableTileAppearance_Instruction(instructionIndex, variableIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer4);
            variableTile.AddAppearances(tileBackground, appearance);

            variableTile.AddLeftClickAction((i) => { GameInterface.FocusVariable(instructionIndex, variableIndex); });
            variableTile.SetEnabledCondition(() => GameInterface.VariableExists(instructionIndex, variableIndex));

            return variableTile;
        }

    }
}
