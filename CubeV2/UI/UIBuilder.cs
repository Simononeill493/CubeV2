using CubeV2.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            rerollButton.Clickable = true;
            rerollButton.OnLeftClick += (i) => { GameInterface.ResetBoardTemplate(); GameInterface.ResetBoard(); GameInterface.PauseBoard(); };

            var goButton = new UIElement(Config.GoButtonName);
            goButton.Appearance = MultiAppearance.Create(new RectangleAppearance(Config.GameControlButtonSize, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"Go"));
            goButton.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth + 500, Config.DefaultScreenHeight - Config.GameControlButtonSize.Y);
            goButton.Clickable = true;
            goButton.OnLeftClick += (i) => GameInterface.StartBoard(TimeSpan.FromSeconds(0.1));


            var resetButton = new UIElement(Config.ResetButtonName);
            resetButton.Appearance = MultiAppearance.Create(new RectangleAppearance(Config.GameControlButtonSize, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"Reset"));
            resetButton.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth + 750, Config.DefaultScreenHeight - Config.GameControlButtonSize.Y);
            resetButton.Clickable = true;
            resetButton.OnLeftClick += (i) => { GameInterface.ResetBoard(); GameInterface.PauseBoard(); };

            var winText = new UIElement(Config.WinTextName);
            winText.Offset = new Vector2(Config.InstructionPanelWidth + Config.SelectorPanelWidth+350, Config.DefaultScreenHeight - (Config.GameControlButtonSize.Y*1.5f));
            winText.Appearance = new TextAppearance(new Color(255,58,200), DrawUtils.UILayer1, "A winner is you!");
            winText.SetEnabledCondition(() => GameInterface.IsGameWon);


            topLevel.AddChildren(instructionPanel, selectorPanel, gameGrid,rerollButton,goButton,resetButton,winText);





            var instructionTiles = _makeInstructionTiles();

            var addInstructionButton = new UIElement(Config.AddInstructionButtonName);
            addInstructionButton.Offset = new Vector2(Config.InstructionPanelWidth * 0.24f, Config.InstructionControlButtonsTopPadding);
            addInstructionButton.Appearance = MultiAppearance.Create(new RectangleAppearance(50, 30, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"+"));
            addInstructionButton.Clickable = true;
            addInstructionButton.OnLeftClick += (i) => GameInterface.AddInstruction();

            var removeInstructionButton = new UIElement(Config.RemoveInstructionButtonName);
            removeInstructionButton.Offset = new Vector2(Config.InstructionPanelWidth * 0.6f, Config.InstructionControlButtonsTopPadding);
            removeInstructionButton.Appearance = MultiAppearance.Create(new RectangleAppearance(50, 30, Color.AliceBlue, DrawUtils.UILayer2), new TextAppearance(Color.Black, DrawUtils.UILayer3,"-"));
            removeInstructionButton.Clickable = true;
            removeInstructionButton.OnLeftClick += (i) => GameInterface.RemoveInstruction();

            instructionPanel.AddChildren(addInstructionButton, removeInstructionButton, instructionTiles);



            var variableGrid = _makeVariablesGrid();
            variableGrid.Offset = new Vector2(20, 20);
            variableGrid.SetEnabledCondition(() => GameInterface.FocusedVariableExists);

            selectorPanel.AddChildren(variableGrid);

            return topLevel;
        }

        private static UIGrid _makeGameGrid()
        {
            var gameTileSize = Config.TileBaseSize * Config.TileScale;
            var gameGrid = UIGrid.Make(Config.GameGridName,Config.GameGridWidth,Config.GameGridHeight, gameTileSize,gameTileSize,Config.GameGridPadding,Config.TileBackgroundColor,DrawUtils.UILayer1,new GameTileAppearanceFactory());

            return gameGrid;
        }

        private static UIGrid _makeVariablesGrid()
        {
            var tilePadding = 3;

            var gameGrid = UIGrid.Make(Config.VariableGridName, 4, 7, Config.TileBaseSize*Config.VariableSelectionTileScale, Config.TileBaseSize * Config.VariableSelectionTileScale, tilePadding, Config.SelectionTileVariableColor, DrawUtils.UILayer2, new VariableTileAppearanceFactory_ForSelectionGrid(Config.VariableSelectionTileScale,DrawUtils.UILayer3));
            gameGrid.TileLeftClicked += (input, index) => GameInterface.AssignValueToFocusedVariable(index);
            return gameGrid;
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
            slot.Appearance = MultiAppearance.Create(slotBackground, new InstructionTileAppearance(instructionIndex));
            slot.Offset = new Vector2(0, (Config.InstructionTileInternalPadding + Config.InstructionTileSize.Y) * instructionIndex);

            slot.Clickable = true;
            slot.OnLeftClick += (i) => { GameInterface.FocusedInstruction = instructionIndex; };


            for (int variableIndex = 0; variableIndex < Config.InstructionMaxNumVariables; variableIndex++)
            {
                var variableTile = new UIElement(Config.InstructionVariableTileName + "_" + instructionIndex + "_" + variableIndex);
                var tileBackground = new RectangleAppearance(Config.TileBaseSize * Config.InstructionTileVariableScale, Config.TileBaseSize * Config.InstructionTileVariableScale, Config.InstructionTileAssignedVariableColor, DrawUtils.UILayer3);
                var appearance = new VariableTileAppearance_Instruction(instructionIndex,variableIndex, Config.InstructionTileVariableScale,DrawUtils.UILayer4);

                variableTile.Offset = new Vector2((Config.TileBaseSize * Config.InstructionTileVariableScale+20) * variableIndex, 20);
                variableTile.Appearance = MultiAppearance.Create(tileBackground, appearance);

                var variableIndexCaptured = variableIndex;

                variableTile.Clickable = true;
                variableTile.AddLeftClickAction((i) => { GameInterface.FocusVariable(instructionIndex, variableIndexCaptured); });
                variableTile.SetEnabledCondition(() => GameInterface.VariableExists(instructionIndex, variableIndexCaptured));

                tileBackground.OverrideColor(() => (GameInterface.FocusedInstruction == instructionIndex & GameInterface.FocusedVariable == variableIndexCaptured) ? Config.InstructionTileAssignedVariableHighlightColor : Config.InstructionTileAssignedVariableColor);

                slot.AddChildren(variableTile);
            }

            return slot;

        }

    }
}
