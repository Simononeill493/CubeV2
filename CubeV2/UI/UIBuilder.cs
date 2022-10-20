using CubeV2.UI.Appearance.InstructionSelectionTileAppearance;
using CubeV2.Utils;
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
            topLevel.Appearance = new NoAppearance();

            var instructionPanel = new UIElement(Config.InstructionPanelName);
            instructionPanel.Offset = new Vector2(0, 0);
            instructionPanel.Appearance = new RectangleAppearance(Config.InstructionPanelWidth, Config.DefaultScreenHeight, Config.InstructionPanelColor, DrawUtils.UILayer1);

            var selectorPanel = new UIElement(Config.SelectorPanelName);
            selectorPanel.Offset = new Vector2(Config.InstructionPanelWidth, 0);
            selectorPanel.Appearance = new RectangleAppearance(Config.SelectorPanelWidth, Config.DefaultScreenHeight, Config.SelectorPanelColor, DrawUtils.UILayer1);

            var gameGrid = _makeGameGrid();
            gameGrid.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth, 0);



            var rerollButton = new UIElement(Config.RerollButtonName);
            rerollButton.Appearance = MultiAppearance.Create(new RectangleAppearance(Config.GameControlButtonSize, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reroll"));
            rerollButton.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth + 250, Config.DefaultScreenHeight - Config.GameControlButtonSize.Y);
            rerollButton.AddLeftClickAction((i) => { GameInterface.ResetBoardTemplate(); GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var goButton = new UIElement(Config.GoButtonName);
            goButton.Appearance = MultiAppearance.Create(new RectangleAppearance(Config.GameControlButtonSize, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"Go"));
            goButton.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth + 500, Config.DefaultScreenHeight - Config.GameControlButtonSize.Y);
            goButton.AddLeftClickAction((i) => GameInterface.StartBoard(TimeSpan.FromSeconds(0.1)));


            var resetButton = new UIElement(Config.ResetButtonName);
            resetButton.Appearance = MultiAppearance.Create(new RectangleAppearance(Config.GameControlButtonSize, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"Reset"));
            resetButton.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth + 750, Config.DefaultScreenHeight - Config.GameControlButtonSize.Y);
            resetButton.AddLeftClickAction((i) => { GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var winText = new UIElement(Config.WinTextName);
            winText.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth+350, Config.DefaultScreenHeight - (Config.GameControlButtonSize.Y*1.5f));
            winText.Appearance = new TextAppearance(new Color(255,58,200), DrawUtils.UILayer1, "A winner is you!");
            winText.SetEnabledCondition(() => GameInterface.IsGameWon);


            topLevel.AddChildren(instructionPanel, selectorPanel, gameGrid,rerollButton,goButton,resetButton,winText);





            var instructionTiles = _makeInstructionTiles();

            var addInstructionButton = new UIElement(Config.AddInstructionButtonName);
            addInstructionButton.Offset = new Vector2(Config.InstructionPanelWidth * 0.24f, Config.InstructionControlButtonsTopPadding);
            addInstructionButton.Appearance = MultiAppearance.Create(new RectangleAppearance(50, 30, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"+"));
            addInstructionButton.AddLeftClickAction((i) => GameInterface.AddInstruction());

            var removeInstructionButton = new UIElement(Config.RemoveInstructionButtonName);
            removeInstructionButton.Offset = new Vector2(Config.InstructionPanelWidth * 0.6f, Config.InstructionControlButtonsTopPadding);
            removeInstructionButton.Appearance = MultiAppearance.Create(new RectangleAppearance(50, 30, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"-"));
            removeInstructionButton.AddLeftClickAction((i) => GameInterface.RemoveInstruction());

            instructionPanel.AddChildren(addInstructionButton, removeInstructionButton, instructionTiles);



            var variableSelectorGrid = _makeVariableSelectorGrid();
            variableSelectorGrid.Offset = new Vector2(20, 20);
            variableSelectorGrid.SetEnabledCondition(() => GameInterface.FocusedVariableExists);

            var instructionSelectorGrid = _makeInstructionSelectorGrid();
            instructionSelectorGrid.Offset = new Vector2(20, 20);
            instructionSelectorGrid.SetEnabledCondition(() => GameInterface.FocusedInstructionExists & !GameInterface.FocusedVariableExists & !GameInterface.FocusedOutputExists);

            selectorPanel.AddChildren(variableSelectorGrid,instructionSelectorGrid);

            return topLevel;
        }

        private static UIGrid _makeGameGrid()
        {
            var gameTileSize = Config.TileBaseSize * Config.TileScale;
            var gameGrid = UIGrid.Make(Config.GameGridName,Config.GameGridWidth,Config.GameGridHeight, gameTileSize,gameTileSize,Config.GameGridPadding,Config.TileBackgroundColor,DrawUtils.UILayer1,new GameTileAppearanceFactory(DrawUtils.UILayer2,DrawUtils.UILayer3));

            return gameGrid;
        }

        private static UIGrid _makeVariableSelectorGrid()
        {
            var gridSize = new Vector2Int(4, 7);
            var tileSize = new Vector2Int(Config.TileBaseSize, Config.TileBaseSize) * Config.VariableSelectionTileScale;
            var tilePadding = 3;
            var backgroundColor = Config.SelectionTileVariableColor;
            var backgroundDrawLayer = DrawUtils.UILayer2;
            var appearanceDrawLayer = DrawUtils.UILayer3;
            var appearanceFactory = new VariableTileAppearanceFactory_ForSelectionGrid(Config.VariableSelectionTileScale, appearanceDrawLayer);

            var variableGrid = UIGrid.Make(Config.VariableGridName, gridSize, tileSize, tilePadding, backgroundColor, backgroundDrawLayer, appearanceFactory);
            variableGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedVariable(index);
            return variableGrid;
        }

        private static UIGrid _makeInstructionSelectorGrid()
        {
            var gridSize = new Vector2Int(1, 9);
            var tileSize = new Vector2Int(Config.InstructionSelectorTileSize);
            var tilePadding = 5;

            var backgroundColor = Config.InstructionSelectorTileColor;
            var backgroundDrawLayer = DrawUtils.UILayer2;
            var appearanceDrawLayer = DrawUtils.UILayer3;
            var appearanceFactory = new InstructionSelectionTileAppearanceFactory(appearanceDrawLayer);

            var instructionGrid = UIGrid.Make(Config.InstructionSelectorGridName, gridSize, tileSize, tilePadding, backgroundColor, backgroundDrawLayer, appearanceFactory);
            instructionGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedInstruction(index);
            return instructionGrid;
        }


        private static UIElement _makeInstructionTiles()
        {
            var slotContainer = new UIElement("SlotContainer");
            slotContainer.Appearance = new NoAppearance();
            slotContainer.Offset = new Vector2(Config.InstructionPanelWidth / 2 - Config.InstructionTileSize.X / 2, Config.InstructionTileTopPadding);

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
            slotBackground.OverrideColor(() => (GameInterface.FocusedInstruction == instructionIndex) ? Config.InstructionTileHighlightColor : Config.InstructionTileColor);
            slot.Appearance = MultiAppearance.Create(slotBackground, new InstructionTileAppearance(instructionIndex,DrawUtils.UILayer3));
            slot.Offset = new Vector2(0, (Config.InstructionTileInternalPadding + Config.InstructionTileSize.Y) * instructionIndex);

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

            return slot;
        }

        private static UIElement _makeInstructionOutputTile(int instructionIndex, int outputIndex)
        {
            var outputTile = new UIElement(Config.InstructionOutputTileName + "_" + instructionIndex + "_" + outputIndex);
            outputTile.Offset = new Vector2(Config.InstructionTileSize.X+1,(outputIndex*50));

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileColor, DrawUtils.UILayer2);
            tileBackground.OverrideColor(() => (GameInterface.FocusedInstruction == instructionIndex & GameInterface.FocusedOutput == outputIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor);

            outputTile.Appearance = MultiAppearance.Create(tileBackground);

            outputTile.AddLeftClickAction((i) => { GameInterface.FocusOutput(instructionIndex, outputIndex); });
            outputTile.SetEnabledCondition(() => GameInterface.OutputExists(instructionIndex, outputIndex));

            return outputTile;
        }

        private static UIElement _makeInstructionVariableTile(int instructionIndex,int variableIndex)
        {
            var variableTile = new UIElement(Config.InstructionVariableTileName + "_" + instructionIndex + "_" + variableIndex);
            variableTile.Offset = new Vector2((Config.TileBaseSize * Config.InstructionTileVariableScale + 20) * variableIndex, 20);

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileAssignedVariableColor, DrawUtils.UILayer3);
            tileBackground.OverrideColor(() => (GameInterface.FocusedInstruction == instructionIndex & GameInterface.FocusedVariable == variableIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor);

            var appearance = new VariableTileAppearance_Instruction(instructionIndex, variableIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer4);
            variableTile.Appearance = MultiAppearance.Create(tileBackground, appearance);

            variableTile.AddLeftClickAction((i) => { GameInterface.FocusVariable(instructionIndex, variableIndex); });
            variableTile.SetEnabledCondition(() => GameInterface.VariableExists(instructionIndex, variableIndex));

            return variableTile;
        }

    }
}
