using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace CubeV2
{
    internal class UIBuilder
    {
        public class CursorOverlayAppearance : RectangleAppearance
        {
            public override Vector2 Size => GameInterface._cameraTileSizeFloat;
            public CursorOverlayAppearance(Color color, float layer) : base(0, 0, color, layer){}
        }

        public class OperationalRangeOverlayAppearance : RectangleAppearance
        {
            public override Vector2 Size => GameInterface._cameraTileSizeFloat * ((Config.PlayerOperationalRadius * 2 + 1));
            public OperationalRangeOverlayAppearance(Color color, float layer) : base(0, 0, color, layer) { }
        }



        public static UIElement GenerateUI()
        {
            var topLevel = new UIElement(Config.UITopLevelName);

            var instructionPanel = UIElementMaker.MakeRectangle(Config.InstructionPanelName, Config.InstructionPanelSize, Config.InstructionPanelOffset, Config.InstructionPanelColor, DrawUtils.UILayer1);

            var selectorPanel = UIElementMaker.MakeRectangle(Config.SelectorPanelName, Config.SelectorPanelSize, Config.SelectorPanelOffset, Config.SelectorPanelColor, DrawUtils.UILayer1);

            var gameGrid = _makeGameGrid();
            gameGrid.SetOffset(Config.InstructionPanelSize.X + Config.SelectorPanelSize.X, 60);

            var cursorOverlayTile = new UIElement(Config.CursorOverlayTileName);
            cursorOverlayTile.AddAppearance(new CursorOverlayAppearance(Color.White * 0.5f, DrawUtils.GameLayer4));


            cursorOverlayTile.AddEnabledCondition(() => AllUIElements.GetUIElement(Config.GameGridName).MouseOver);
            gameGrid.AddChildren(cursorOverlayTile);

            var operationalRangeOverlayTile = new UIElement(Config.OperationalRangeOverlayTileName);
            operationalRangeOverlayTile.AddAppearance(new OperationalRangeOverlayAppearance(Color.White * 0.05f, DrawUtils.GameLayer5));
            operationalRangeOverlayTile.AddEnabledCondition(() => GameInterface._game.FocusEntity != null);
            gameGrid.AddChildren(operationalRangeOverlayTile);

            var goButton = UIElementMaker.MakeRectangle(Config.GoButtonName, Config.GameControlButtonSize, Config.GoButtonOffset, Color.Lime, DrawUtils.UILayer2);
            goButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Go"));
            goButton.AddLeftClickAction((i) => GameInterface.StartBoard(Config.DemoBoardUpdateRate));

            var resetButton = UIElementMaker.MakeRectangle(Config.ResetButtonName, Config.GameControlButtonSize, Config.ResetButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            resetButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reset"));
            resetButton.AddLeftClickAction((i) => { GameInterface.ResetBoard(); GameInterface.StartBoard(Config.DemoBoardUpdateRate); });

            var rerollButton = UIElementMaker.MakeRectangle(Config.RerollButtonName, Config.GameControlButtonSize, Config.RerollButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            rerollButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reroll"));
            rerollButton.AddLeftClickAction((i) => { GameInterface.RerollBoard(); GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var simulateButton = UIElementMaker.MakeRectangle(Config.SimulateButtonName, Config.SimulateButtonSize,Config.SimulateButtonOffset,Color.White,DrawUtils.UILayer2);
            simulateButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Simulate"));
            simulateButton.AddLeftClickAction((i) => { GameInterface.SimulateCurrentGame(i); });

            var controlButtonArray = new UIElement("ControlButtonArray");
            controlButtonArray.SetOffset(new Vector2(Config.InstructionPanelSize.X + Config.SelectorPanelSize.X + 800, (int)Config.ScreenSize.Y - Config.GameControlButtonSize.Y - 15));
            controlButtonArray.AddChildren(goButton, resetButton, rerollButton, simulateButton);


            var energyBar = new UIElement(Config.EnergyBarName);
            energyBar.SetOffset(Config.EnergyBarOffset);
            energyBar.AddAppearance(new EnergyBarAppearance(Config.EnergyBarSize, DrawUtils.UILayer1, DrawUtils.UILayer2));

            var displayText = new UIElement(Config.DisplayTextName);
            displayText.SetOffset(Config.DisplayTextOffset);
            displayText.AddAppearance(new TextAppearance(new Color(255,58,200), DrawUtils.UILayer1, ()=>GameInterface.DisplayText));
            //displayText.SetEnabledCondition(() => GameInterface.IsGameWon);

            var playerActionArray = CreatePlayerActionArray();
            playerActionArray.SetOffset(520, (int)Config.ScreenSize.Y - Config.GameControlButtonSize.Y - 15);
                

            topLevel.AddChildren(instructionPanel, selectorPanel, gameGrid, controlButtonArray, energyBar,displayText,playerActionArray);



            var instructionTiles = _makeInstructionTiles();

            var addInstructionButton = UIElementMaker.MakeRectangle(Config.AddInstructionButtonName, new Vector2(50,30), Config.AddInstructionButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            addInstructionButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3,"+"));
            addInstructionButton.AddLeftClickAction((i) => GameInterface.AddInstructionToEnd());

            var removeInstructionButton = UIElementMaker.MakeRectangle(Config.RemoveInstructionButtonName, new Vector2(50, 30), Config.RemoveInstructionButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            removeInstructionButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3,"-"));
            removeInstructionButton.AddLeftClickAction((i) => GameInterface.RemoveInstructionFromEnd());

            instructionPanel.AddChildren(addInstructionButton, removeInstructionButton, instructionTiles);

            var instructionSelectorGrid = _makeInstructionSelectorGrid();
            instructionSelectorGrid.SetOffset(20, 20);
            instructionSelectorGrid.AddEnabledCondition(InstructionSelectorGridEnabled);

            var variableSelectorGrid = _makeVariableSelectorGrid();
            variableSelectorGrid.SetOffset(20, 20);
            variableSelectorGrid.AddEnabledCondition(VariableSelectorGridEnabled);

            var outputSelectorGrid = _makeOutputSelectorGrid();
            outputSelectorGrid.SetOffset(20, 20);
            outputSelectorGrid.AddEnabledCondition(OutputSelectorGridEnabled);

            var tileViewer = _makeTileViewer();
            tileViewer.SetOffset(20, 20);
            tileViewer.AddEnabledCondition(TileViewerEnabled);

            selectorPanel.AddChildren(instructionSelectorGrid,variableSelectorGrid,outputSelectorGrid, tileViewer);

            return topLevel;
        }

        public static UIElement CreatePlayerActionArray()
        {
            var actionArray = new UIElement("ActionArray");

            var currentPlayerAction = UIElementMaker.MakeRectangle("PlayerActionButton", Config.GameControlButtonSize, Vector2.Zero, Color.LightGray, DrawUtils.UILayer2);
            currentPlayerAction.AddAppearance(new TextAppearance(new Color(179,14,55), DrawUtils.UILayer3, ()=>PlayerActionUtils.GetDisplayMessage(GameInterface.SelectedPlayerAction)));

            var currentSubSelection = UIElementMaker.MakeRectangle("PlayerActionSubSelectionButton", Config.GameControlButtonSize, new Vector2(120,0), Color.White, DrawUtils.UILayer2);
            currentSubSelection.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Go"));
            currentSubSelection.AddEnabledCondition(()=>PlayerActionUtils.HasSubSelection(GameInterface.SelectedPlayerAction));

            actionArray.AddChildren(currentPlayerAction, currentSubSelection);

            return actionArray;
        }

        private static bool InstructionSelectorGridEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.Instruction || GameInterface.CurrentSidePanelFocus == SidePanelFocus.InstructionOption) && GameInterface.FocusedInstructionExists;
        }

        private static bool VariableSelectorGridEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.Variable || GameInterface.CurrentSidePanelFocus == SidePanelFocus.VariableOption) && GameInterface.FocusedVariableExists;
        }

        private static bool OutputSelectorGridEnabled()
        {
            return GameInterface.CurrentSidePanelFocus == SidePanelFocus.Output && GameInterface.FocusedOutputExists;
        }

        private static bool TileViewerEnabled()
        {
            return GameInterface.CurrentSidePanelFocus == SidePanelFocus.Tile && GameInterface.FocusedTileExists;
        }


        private static UIGrid _makeGameGrid()
        {
            var appearanceFactory = new GameTileAppearanceFactory(DrawUtils.GameLayer1, DrawUtils.GameLayer2);

            var gameGrid = new UIGrid(Config.GameGridName,Config.GameUIGridMaxGridWidth,Config.GameUIGridMaxGridHeight, appearanceFactory);
            //gameGrid.Arrange(Config.GameUIGridDefaultWidth, Config.GameUIGridDefaultHeight, (int)gameTileSize.X, (int)gameTileSize.Y, Config.GameUIGridPadding);
            //gameGrid.Arrange(GameInterface.CameraSize.X, GameInterface.CameraSize.Y, (int)gameTileSize.X*2, (int)gameTileSize.Y*2, Config.GameUIGridPadding);

            //gameGrid.TileLeftClicked += (i, index) => Pause();
            gameGrid.TileLeftClicked += (i, index) => GameInterface.LeftClickBoard(index);
            gameGrid.TileRightClicked += (i, index) => GameInterface.RightClickBoard(index);

            return gameGrid;
        }

        private static UIElement _makeTileViewer()
        {
            var container = new UIElement("TileViewer");

            var tileDetailsPanel = UIElementMaker.MakeRectangle("TileDetailsPanel", new Vector2(210, 80), Vector2.Zero, new Color(212, 161, 161), DrawUtils.UILayer2);

            var tilePicture = new UIElement("TilePicture");
            tilePicture.AddAppearance(new SpriteAppearance(DrawUtils.UILayer3, DrawUtils.GroundSprite) { Scale = 3 });
            tilePicture.SetOffset(25, 10);

            var tileCoordsText = new UIElement("TileCoordsText");
            tileCoordsText.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3,()=>GameInterface.GetFocusedTileCoordinates().ToString()));
            tileCoordsText.SetOffset(120, 10);

            tileDetailsPanel.AddChildren(tilePicture,tileCoordsText);



            var contentsDetailsPanel = UIElementMaker.MakeRectangle("ContentsDetailsPanel", new Vector2(210, 150), Vector2.Zero, new Color(212, 161, 161), DrawUtils.UILayer2);
            contentsDetailsPanel.AddEnabledCondition(GameInterface.FocusedTileHasEntity);
            contentsDetailsPanel.SetOffset(0,120);

            var contentsPicture = new UIElement("ContentsPicture");
            contentsPicture.AddAppearance(new SpriteAppearance(DrawUtils.UILayer3, ()=>GameInterface.GetFocusedTileEntity().Sprite) { Scale = 3 });
            contentsPicture.SetOffset(25, 10);

            var contentsNameText = new UIElement("TileContentsNameText");
            contentsNameText.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, () => GameInterface.GetFocusedTileEntity().GetEntityName())); ;
            contentsNameText.SetOffset(25, 80);

            var contentsEnergyText = new UIElement("TileContentsEnergyText");
            contentsEnergyText.AddAppearance(new TextAppearance(Color.DarkGreen, DrawUtils.UILayer3, () => GameInterface.GetFocusedTileEntity().GetEnergyOverMaxAsText())); ;
            contentsEnergyText.SetOffset(25, 100);


            contentsDetailsPanel.AddChildren(contentsPicture,contentsNameText, contentsEnergyText);


            container.AddChildren(tileDetailsPanel, contentsDetailsPanel);

            return container;
        }

        private static UIGrid _makeVariableSelectorGrid()
        {
            var tileSize = Config.TileBaseSize * Config.VariableSelectionTileScale;
            var tilePadding = 3;
            var appearanceFactory = new VariableTileAppearanceFactory_ForSelectionGrid(Config.VariableSelectionTileScale, DrawUtils.UILayer2,DrawUtils.UILayer3);

            var variableGrid = new UIGrid(Config.VariableGridName, Config.VariableSelectorGridSize, appearanceFactory);
            variableGrid.Arrange(Config.VariableSelectorGridSize, new Vector2Int(tileSize), tilePadding);

            variableGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedVariable(index);
            return variableGrid;
        }

        private static UIGrid _makeInstructionSelectorGrid()
        {
            var gridSize = new Vector2Int(1, 18);
            var tileSize = new Vector2Int(Config.InstructionOptionTileSize);
            var tilePadding = 5;

            var appearanceFactory = new InstructionSelectionTileAppearanceFactory(DrawUtils.UILayer2, DrawUtils.UILayer3);

            var instructionGrid = new UIGrid(Config.InstructionSelectorGridName, gridSize, appearanceFactory);
            instructionGrid.Arrange(gridSize, tileSize, tilePadding);

            instructionGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedInstruction(index);
            return instructionGrid;
        }

        private static UIGrid _makeOutputSelectorGrid()
        {
            var gridSize = new Vector2Int(1, Config.EntityMaxVariables);
            var tileSize = new Vector2Int(Config.InstructionOptionTileSize);
            var tilePadding = 10;

            var appearanceFactory = new OutputSelectionTileAppearanceFactory(Config.VariableSelectionTileScale, DrawUtils.UILayer2, DrawUtils.UILayer3);

            var outputSelectorGrid = new UIGrid(Config.OutputSelectorGridName, gridSize, appearanceFactory);
            outputSelectorGrid.Arrange(gridSize, tileSize, tilePadding);
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
            indexText.SetOffset(-20, Config.InstructionTileSize.Y / 5);
            slot.AddChildren(indexText);

            var deleteButton = new UIElement(slot.ID + "_deleteButton");
            deleteButton.AddAppearance(MultiAppearance.Create(new RectangleAppearance(20, 20, Color.White, DrawUtils.UILayer4), new TextAppearance(Color.Red, DrawUtils.UILayer5, "X")));
            deleteButton.SetOffset(-20, Config.InstructionTileSize.Y-20);
            deleteButton.AddLeftClickAction((i) => GameInterface.RemoveInstructionAtIndex(instructionIndex));
            slot.AddChildren(deleteButton);

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
            outputTile.AddEnabledCondition(() => GameInterface.OutputExists(instructionIndex, outputIndex));

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
            outputTile.AddEnabledCondition(() => GameInterface.ControlOutputExists(instructionIndex, controlIndex));
            outputTile.AddKeyPressedAction((i) => { if (i.IsNumberJustPressed) { GameInterface.AssignValueToFocusedControlOutput(i.GetNumberJustPressed); } });

            return outputTile;
        }


        private static UIElement _makeInstructionVariableTile(int instructionIndex,int variableIndex)
        {
            var variableTile = new UIElement(Config.InstructionVariableTileName + "_" + instructionIndex + "_" + variableIndex);
            variableTile.SetOffset((Config.TileBaseSize.X * Config.InstructionTileVariableScale + 20) * variableIndex, 20);

            var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileAssignedVariableColor, DrawUtils.UILayer3);
            tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnVariable(instructionIndex,variableIndex)  ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));

            var appearance = new VariableTileAppearance_Instruction(instructionIndex, variableIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer4);
            variableTile.AddAppearances(tileBackground, appearance);

            variableTile.AddLeftClickAction((i) => { GameInterface.FocusVariable(instructionIndex, variableIndex); });
            variableTile.AddEnabledCondition(() => GameInterface.VariableExists(instructionIndex, variableIndex));

            return variableTile;
        }

    }
}
