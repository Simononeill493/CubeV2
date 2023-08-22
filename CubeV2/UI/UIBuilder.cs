using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace CubeV2
{
    internal partial class UIBuilder
    {
        public static UIElement GenerateUI()
        {
            var gameTopBar = MakeGameTopBar();
            var gameBottomBar = MakeGameBottomBar();
            var gameBoard = MakeGameBoard();
            var gameSidePanels = MakeSidePanels();

            var topLevel = new UIElement(Config.UITopLevelName);
            topLevel.AddChildren(gameSidePanels, gameBoard, gameBottomBar, gameTopBar);
            return topLevel;
        }

        public static UIElement MakeGameBoard()
        {
            var gameBoard = _makeGameBoard();
            //gameBoard.SetOffset(Config.InstructionPanelSize.X + Config.SelectorPanelSize.X, 60);
            gameBoard.SetOffset(Config.SelectorPanelSize.X, 0);
            gameBoard.AddLeftClickAction((i) => GameInterface.PrimaryFocus = PrimaryFocus.Board);

            return gameBoard;
        }

        public static UIElement MakeGameBottomBar()
        {
            var goButton = UIElementMaker.MakeRectangle(Config.GoButtonName, Config.GameControlButtonSize, Config.GoButtonOffset, Color.Lime, DrawUtils.UILayer2);
            goButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Go"));
            goButton.AddLeftClickAction((i) => GameInterface.StartBoard(Config.BoardMasterUpdateRate));

            var resetButton = UIElementMaker.MakeRectangle(Config.ResetButtonName, Config.GameControlButtonSize, Config.ResetButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            resetButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reset"));
            resetButton.AddLeftClickAction((i) => { GameInterface.ResetBoard(); GameInterface.StartBoard(Config.BoardMasterUpdateRate); });

            var rerollButton = UIElementMaker.MakeRectangle(Config.RerollButtonName, Config.GameControlButtonSize, Config.RerollButtonOffset, Color.AliceBlue, DrawUtils.UILayer2);
            rerollButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Reroll"));
            rerollButton.AddLeftClickAction((i) => { GameInterface.RerollBoard(); GameInterface.ResetBoard(); GameInterface.PauseBoard(); });

            var simulateButton = UIElementMaker.MakeRectangle(Config.SimulateButtonName, Config.SimulateButtonSize, Config.SimulateButtonOffset, Color.White, DrawUtils.UILayer2);
            simulateButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "Simulate"));
            simulateButton.AddLeftClickAction((i) => { GameInterface.SimulateCurrentGame(i); });


            var controlBarArray = new UIElement("ControlButtonArray");
            controlBarArray.SetOffset(new Vector2(Config.InstructionPanelSize.X + Config.SelectorPanelSize.X + 25, (int)Config.ScreenSize.Y - Config.GameControlButtonSize.Y - 15));
            controlBarArray.AddChildren(MakePlayerClickActionButtons(), goButton, resetButton, rerollButton, simulateButton);

            return controlBarArray;
        }

        public static UIElement MakePlayerClickActionButtons()
        {
            var clickActionButtonArray = new UIElement("ClickActionController");

            var currentPlayerClickAction = UIElementMaker.MakeRectangle("PlayerClickActionButton", Config.GameControlButtonSize, Vector2.Zero, Color.LightGray, DrawUtils.UILayer2);
            currentPlayerClickAction.AddAppearance(new TextAppearance(new Color(179,14,55), DrawUtils.UILayer3, ()=>PlayerActionUtils.GetDisplayMessage(GameInterface.SelectedPlayerAction)));

            var currentClickActionSubSelection = UIElementMaker.MakeRectangle("PlayerClickActionSubSelectionButton", Config.GameControlButtonSize, new Vector2(120,0), Color.White, DrawUtils.UILayer2);
            currentClickActionSubSelection.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, "SubSelection"));
            currentClickActionSubSelection.AddEnabledCondition(()=>PlayerActionUtils.HasSubSelection(GameInterface.SelectedPlayerAction));

            clickActionButtonArray.AddChildren(currentPlayerClickAction, currentClickActionSubSelection);

            return clickActionButtonArray;
        }

        public static UIElement MakeGameTopBar()
        {
            var displayText = new UIElement(Config.DisplayTextName);
            displayText.AddAppearance(new TextAppearance(new Color(255, 58, 200), DrawUtils.UILayer1, () => GameInterface.DisplayText));
            displayText.SetOffset(10, 10);
            //displayText.SetEnabledCondition(() => GameInterface.IsGameWon);

            var healthBar = new UIElement(Config.HealthBarName);
            healthBar.SetOffset(new Vector2(10, 35));
            healthBar.AddAppearance(new HealthBarAppearance(Config.HealthBarSize, DrawUtils.UILayer1, DrawUtils.UILayer2));

            var energyBar = new UIElement(Config.EnergyBarName);
            energyBar.SetOffset(new Vector2(10, 70));
            energyBar.AddAppearance(new EnergyBarAppearance(Config.EnergyBarSize, DrawUtils.UILayer1, DrawUtils.UILayer2));




            var topBarContainer = new UIElement("TopBar");
            topBarContainer.SetOffset(new Vector2(Config.ScreenSize.X - (Config.HealthBarSize.X + 80), 10));
            topBarContainer.AddChildren(healthBar,energyBar, displayText);
            topBarContainer.AddAppearance(new RectangleAppearance(570, 100, Color.Black, DrawUtils.UILayerBackground));

            return topBarContainer;
        }

        public static UIElement MakeSidePanels()
        {
            //var instructionPanel = MakeInstructionPanel(Config.InstructionPanelOffset);
            //var selectorPanel = MakeSelectorPanel(Config.SelectorPanelOffset);
            var selectorPanel = MakeSelectorPanel(Config.InstructionPanelOffset);

            var sidePanelContainer = new UIElement("SidePanelContainer");
            //sidePanelContainer.AddChildren(instructionPanel, selectorPanel);
            sidePanelContainer.AddChildren(selectorPanel);

            return sidePanelContainer;
        }



        private static UIElement _makeInstructionSetManager()
        {
            var leftArrow = new UIElement("InstructionSetLeftArrow");
            leftArrow.AddAppearance(new RectangleAppearance(new Vector2(18,32),Color.Gray,DrawUtils.UILayer3));
            leftArrow.AddAppearance(new SpriteAppearance(DrawUtils.UILayer4,DrawUtils.MenuArrow1) { Scale = 2, FlipHorizontal = true });
            leftArrow.AddLeftClickAction((i) => GameInterface.FocusInstructionSet(GameInterface.FocusedInstructionSet-1));
            leftArrow.SetOffset(0, 0);

            var rightArrow = new UIElement("InstructionSetRightArrow");
            rightArrow.AddAppearance(new RectangleAppearance(new Vector2(18, 32), Color.Gray, DrawUtils.UILayer3));
            rightArrow.AddAppearance(new SpriteAppearance(DrawUtils.UILayer4, DrawUtils.MenuArrow1) { Scale = 2});
            rightArrow.AddLeftClickAction((i) => GameInterface.FocusInstructionSet(GameInterface.FocusedInstructionSet + 1));
            rightArrow.SetOffset(100, 0);

            var focusNumber = new UIElement("InstructionSetFocusNumber");
            focusNumber.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer3, ()=>GameInterface.FocusedInstructionSet.ToString()));
            focusNumber.SetOffset(50, 0);

            var makeNewSetButton= new UIElement("InstructionSetCreateNewButton");
            makeNewSetButton.AddAppearance(new RectangleAppearance(new Vector2(18, 32), Color.Gray, DrawUtils.UILayer3));
            makeNewSetButton.AddAppearance(new TextAppearance(Color.Black, DrawUtils.UILayer4,"+"));
            makeNewSetButton.AddLeftClickAction((i) =>GameInterface.AddInstructionSet());
            makeNewSetButton.SetOffset(150, 0);


            var container = new UIElement("InstructionSetManager");
            container.AddChildren(leftArrow, rightArrow, focusNumber, makeNewSetButton);
            return container;
        }

        private static UIGrid _makeGameBoard()
        {
            var appearanceFactory = new GameTileAppearanceFactory(DrawUtils.GameLayer1, DrawUtils.GameLayer2,DrawUtils.GameLayer4,DrawUtils.GameLayer5);
            var gameGrid = new UIGameGrid(Config.GameGridName,Config.GameUIGridMaxGridIndexWidth,Config.GameUIGridMaxGridIndexHeight, appearanceFactory);
            //gameGrid.Arrange(Config.GameUIGridDefaultWidth, Config.GameUIGridDefaultHeight, (int)gameTileSize.X, (int)gameTileSize.Y, Config.GameUIGridPadding);
            //gameGrid.Arrange(GameInterface.CameraSize.X, GameInterface.CameraSize.Y, (int)gameTileSize.X*2, (int)gameTileSize.Y*2, Config.GameUIGridPadding);

            return gameGrid;
        }

        private static UIElement _makeTileViewer()
        {
            var tileViewerContainer = new UIElement("TileViewer");

            var tileDetailsPanel = UIElementMaker.MakeRectangle("TileDetailsPanel", new Vector2(210, 80), Vector2.Zero, new Color(212, 161, 161), DrawUtils.UILayer2);

            var tilePicture = new UIElement("TilePicture");
            tilePicture.AddAppearance(new SpriteAppearance(DrawUtils.UILayer3, DrawUtils.CircuitGround1) { Scale = 3 });
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


            tileViewerContainer.AddChildren(tileDetailsPanel, contentsDetailsPanel);
            tileViewerContainer.AddEnabledCondition(TileViewerEnabled);

            return tileViewerContainer;
        }

        public static UIElement MakeInstructionPanel(Vector2 offset)
        {
            var instructionPanel = UIElementMaker.MakeRectangle(Config.InstructionPanelName, Config.InstructionPanelSize, offset, Config.InstructionPanelColor, DrawUtils.UILayer1);
            instructionPanel.AddLeftClickAction((i) => GameInterface.PrimaryFocus = PrimaryFocus.Editor);

            var instructionTiles = _makeInstructionTiles();
            instructionTiles.SetOffset(Config.InstructionPanelSize.X / 2 - Config.InstructionTileSize.X / 2, Config.InstructionTileTopPadding);

            var instructionSetManager = _makeInstructionSetManager();
            instructionSetManager.SetOffset(65, 10);
            instructionPanel.AddChildren(instructionTiles, instructionSetManager);

            return instructionPanel;
        }

        public static UIElement MakeSelectorPanel(Vector2 offset)
        {
            var selectorPanel = UIElementMaker.MakeRectangle(Config.SelectorPanelName, Config.SelectorPanelSize, offset, Config.SelectorPanelColor, DrawUtils.UILayer1);
            selectorPanel.AddLeftClickAction((i) => GameInterface.PrimaryFocus = PrimaryFocus.Editor);

            var listContainer = new UIElement("SelectorPanelListContainer");
            listContainer.SetOffset(20, 20);
            listContainer.AddChildren(_makeListOfInstructions(), _makeListOfVariableCategories(), _makeOutputSelectorGrid(), _makeTileViewer(), _makeGenericVariableList(), _makeIntegerVariableMaker());
            selectorPanel.AddChildren(listContainer);


            return selectorPanel;
        }

        private static UIGrid _makeListOfInstructions()
        {
            var appearanceFactory = new InstructionSelectionTileAppearanceFactory(DrawUtils.UILayer2, DrawUtils.UILayer3);

            var listOfInstructions = new UIGrid(Config.ListOfInstructionsName, Config.ListOfInstructionsGridSize, appearanceFactory);
            listOfInstructions.Arrange(Config.ListOfInstructionsGridSize, new Vector2Int(Config.InstructionOptionTileSize), 5);
            listOfInstructions.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedInstruction(index);
            listOfInstructions.AddEnabledCondition(ListOfInstructionsEnabled);

            return listOfInstructions;
        }

        private static UIGrid _makeListOfVariableCategories()
        {
            var appearanceFactory = new VariableCategoryAppearanceFactory(Config.VariableSelectionTileScale, DrawUtils.UILayer2,DrawUtils.UILayer3);

            var listOfVariableCategories = new UIGrid(Config.VariableCategoryListName, Config.VariableCategoryListSize, appearanceFactory);
            listOfVariableCategories.Arrange(Config.VariableCategoryListSize, new Vector2Int(Config.VariableCategoryTileSize), 3);
            listOfVariableCategories.TileLeftClicked += (input, index) => GameInterface.VaribleCategorySelected(index);
            listOfVariableCategories.AddEnabledCondition(VariableCategoryListEnabled);

            return listOfVariableCategories;
        }

        private static UIGrid _makeGenericVariableList()
        {
            var gridSize = new Vector2Int(4, 7);
            var appearanceFactory = new VariableTileAppearanceFactoryGrid(DrawUtils.UILayer2, DrawUtils.UILayer3,3);

            var listOfVariables = new UIGrid("ListOfVariables", gridSize, appearanceFactory);
            listOfVariables.Arrange(gridSize, Config.TileBaseSizeInt * Config.VariableSelectionTileScale, 5);
            listOfVariables.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedVariableFromGrid(index);
            listOfVariables.AddEnabledCondition(GenericVariableListEnabled);

            return listOfVariables;
        }

        private static UIElement _makeIntegerVariableMaker()
        {
            var integerTextBox = new TextBox(Config.IntegerTextBoxName,200,25,DrawUtils.UILayer2,DrawUtils.UILayer3);
            integerTextBox.IsKeyValid = (c) => c.IsNumeric();

            var doneButton = new TextBox("IntegerVariableConfirmButton", 150, 20, DrawUtils.UILayer2, DrawUtils.UILayer3);
            doneButton.SetOffset(0, 75);
            doneButton.AddLeftClickAction((i) => GameInterface.TryAssignIntegerValueToFocusedVariable(integerTextBox.Text));
            doneButton.Text = "Confirm";
            doneButton.Editable = false;

            var integerMakerContainer = new UIElement("IntegerVariableMaker");
            integerMakerContainer.AddEnabledCondition(IntegerVariableMakerEnabled);
            integerMakerContainer.AddChildren(integerTextBox,doneButton);
            return integerMakerContainer;
        }



        private static UIGrid _makeOutputSelectorGrid()
        {
            var appearanceFactory = new OutputSelectionTileAppearanceFactory(Config.VariableSelectionTileScale, DrawUtils.UILayer2, DrawUtils.UILayer3);

            var outputSelectorGrid = new UIGrid(Config.OutputSelectorGridName, Config.OutputSelectorGridSize, appearanceFactory);
            outputSelectorGrid.Arrange(Config.OutputSelectorGridSize, new Vector2Int(Config.InstructionOptionTileSize), 10);
            outputSelectorGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedOutput(index);
            outputSelectorGrid.AddEnabledCondition(OutputSelectorGridEnabled);

            return outputSelectorGrid;
        }

        private static UIElement _makeInstructionTiles()
        {
            var slotContainer = new UIElement("SlotContainer");

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

            for (int controlIndex = 0; controlIndex < Config.InstructionMaxNumControlFlowOutputs; controlIndex++)
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
            outputTile.SetOffset(Config.InstructionTileSize.X+1,75+(outputIndex*50));

            var tileBackground = new RectangleAppearance(Config.OutputTileBaseSize * (Config.InstructionTileVariableScale-2), new Color(251,167,232), DrawUtils.UILayer2);
            //tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnOutput(instructionIndex,outputIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));
            var tileAppearance = new OutputTileAppearance(outputIndex, instructionIndex, Config.InstructionTileVariableScale-2, DrawUtils.UILayer3);
            outputTile.AddAppearances(tileBackground,tileAppearance);

            outputTile.AddLeftClickAction((i) => { GameInterface.FocusOutput(instructionIndex, outputIndex); });
            outputTile.AddEnabledCondition(() => GameInterface.OutputExists(instructionIndex, outputIndex));

            return outputTile;
        }

        private static UIElement _makeInstructionControlTile(int instructionIndex, int controlIndex)
        {
            var outputTile = new UIElement(Config.InstructionControlOutputTileName + "_" + instructionIndex + "_" + controlIndex);
            outputTile.SetOffset(Config.InstructionTileSize.X + 1, (controlIndex * 25));

            var tileBackground = new RectangleAppearance(Config.OutputTileBaseSize * (Config.InstructionTileVariableScale-2), new Color(156,219,250), DrawUtils.UILayer2);
            //tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnControlOutput(instructionIndex, controlIndex) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));
            var tileAppearance = new OutputControlTileAppearance(controlIndex, instructionIndex, Config.InstructionTileVariableScale-2, DrawUtils.UILayer3);
            outputTile.AddAppearances(tileBackground, tileAppearance);

            outputTile.AddLeftClickAction((i) => { GameInterface.FocusControlOutput(instructionIndex, controlIndex); });
            outputTile.AddEnabledCondition(() => GameInterface.ControlOutputExists(instructionIndex, controlIndex));
            outputTile.AddKeyPressedAction((i) => { if (GameInterface.CurrentSidePanelFocus == SidePanelFocus.ControlOutput && i.IsNumberJustPressed) { GameInterface.AssignValueToFocusedControlOutput(i.GetNumberJustPressed); } });

            return outputTile;
        }

        private static UIElement _makeInstructionVariableTile(int instructionIndex,int variableIndex)
        {
            var variableTile = new UIElement(Config.InstructionVariableTileName + "_" + instructionIndex + "_" + variableIndex);
            variableTile.SetOffset((Config.TileBaseSizeFloat.X * Config.InstructionTileVariableScale + 20) * variableIndex, 20);

            var tileBackground = new RectangleAppearance(Config.TileBaseSizeFloat * Config.InstructionTileVariableScale, Config.InstructionTileAssignedVariableColor, DrawUtils.UILayer3);
            tileBackground.OverrideColor(() => (GameInterface.IsFocusedOnVariable(instructionIndex,variableIndex)  ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor));

            var appearance = new VariableTileAppearance_InInstruction(instructionIndex, variableIndex, Config.InstructionTileVariableScale, DrawUtils.UILayer4);
            variableTile.AddAppearances(tileBackground, appearance);

            variableTile.AddLeftClickAction((i) => { GameInterface.FocusVariable(instructionIndex, variableIndex); });
            variableTile.AddEnabledCondition(() => GameInterface.VariableExists(instructionIndex, variableIndex));

            return variableTile;
        }

        private static bool ListOfInstructionsEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.Instruction || GameInterface.CurrentSidePanelFocus == SidePanelFocus.InstructionOption);
        }

        private static bool VariableCategoryListEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.VariableCategory || GameInterface.CurrentSidePanelFocus == SidePanelFocus.VariableOption) && GameInterface.FocusedVariableExists;
        }

        private static bool GenericVariableListEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.VariableGeneric);
        }

        private static bool IntegerVariableMakerEnabled()
        {
            return (GameInterface.CurrentSidePanelFocus == SidePanelFocus.IntegerVariableMaker);
        }

        private static bool OutputSelectorGridEnabled()
        {
            return GameInterface.CurrentSidePanelFocus == SidePanelFocus.Output && GameInterface.FocusedOutputExists;
        }

        private static bool TileViewerEnabled()
        {
            return GameInterface.CurrentSidePanelFocus == SidePanelFocus.Tile && GameInterface.FocusedTileExists;
        }
    }
}
