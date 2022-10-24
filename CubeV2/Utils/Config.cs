﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class Config
    {
        public static bool KnowAllInstructionsByDefault = true;
        public static int EntityMaxVariables = 3;

        public static TimeSpan DemoBoardUpdateRate = TimeSpan.FromSeconds(0.1);

        //public static int GameGridWidth = 1;
        //public static int GameGridHeight = 1;
        public static int GameGridWidth = 14;
        public static int GameGridHeight = 10;

        public static int GameGridPadding = 2;

        public static Vector2 TileBaseSize = new Vector2(16,16);
        public static int TileScale = 4;
        public static int VariableSelectionTileScale = 4;
        public static int InstructionTileVariableScale = 3;


        public static int NumInstructionTiles = 6;
        public static int InstructionTileInternalPadding = 10;
        public static int InstructionTileTopPadding = 50;
        public static int InstructionMaxNumVariables = 3;
        public static int InstructionMaxNumOutputs = 2;
        public static int InstructionMaxNumControlOutputs = 2;
        public static int MaxInstructionJumpsPerTick = 50;




        public static string UITopLevelName = "UITopLevel";
        public static string InstructionPanelName = "InstructionPanel";
        public static string SelectorPanelName = "SelectorPanel";
        public static string InstructionSlotName = "InstructionSlot";
        public static string InstructionOutputTileName = "InstructionOutputTile";
        public static string InstructionControlOutputTileName = "InstructionControlOutputTile";
        public static string InstructionName = "Instruction";
        public static string AddInstructionButtonName = "AddInstructionButton";
        public static string RemoveInstructionButtonName = "RemoveInstructionButton";
        public static string RerollButtonName = "RerollButton";
        public static string GoButtonName = "GoButton";
        public static string ResetButtonName = "ResetButton";
        public static string GameGridName = "GameGrid";
        public static string VariableGridName = "VariableGrid";
        public static string InstructionSelectorGridName = "InstructionSelectorGrid";
        public static string InstructionVariableTileName = "InstructionVariableTile";
        public static string OutputSelectorGridName = "OutputSelectorGrid";

        public static string WinTextName = "WinText";

        public static string GoalTag = "EnteredGoal";

        public static void Load() 
        {
            _loadPositioning();
        }

        public static Vector2 ScreenSize = new Vector2(1550, 800);

        public static Vector2 InstructionPanelSize;
        public static Vector2 InstructionPanelOffset;

        public static Vector2 SelectorPanelSize;
        public static Vector2 SelectorPanelOffset;

        public static Vector2 InstructionTileSize;
        public static Vector2 InstructionOptionTileSize;

        public static Vector2 AddInstructionButtonOffset;
        public static Vector2 RemoveInstructionButtonOffset;
        public static int InstructionControlButtonsTopPadding = 15;


        public static Vector2 GameControlButtonSize;
        public static Vector2 RerollButtonOffset;
        public static Vector2 GoButtonOffset;
        public static Vector2 ResetButtonOffset;

        public static Vector2 WinTextOffset;


        private static void _loadPositioning()
        {
            InstructionPanelSize = new Vector2(300, ScreenSize.Y);
            InstructionPanelOffset = Vector2.Zero;

            SelectorPanelSize = new Vector2(300, ScreenSize.Y);
            SelectorPanelOffset = new Vector2(Config.InstructionPanelSize.X, 0);

            InstructionTileSize = new Vector2(200, 100);
            InstructionOptionTileSize = new Vector2(200, 75);

            AddInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.24f, InstructionControlButtonsTopPadding);
            RemoveInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.6f, InstructionControlButtonsTopPadding);

            GameControlButtonSize = new Vector2(100, 60);
            RerollButtonOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 250, (int)ScreenSize.Y - GameControlButtonSize.Y);
            GoButtonOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 500, (int)ScreenSize.Y - GameControlButtonSize.Y);
            ResetButtonOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 750, (int)ScreenSize.Y - GameControlButtonSize.Y);

            WinTextOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 350, (int)ScreenSize.Y - (GameControlButtonSize.Y * 1.5f));

        }






        public static Color InstructionSelectorTileColor = Color.White;
        public static Color PrimaryBackgroundColor = new Color(80, 63, 63);
        public static Color TileBackgroundColor = new Color(32, 26, 26);
        public static Color InstructionPanelColor = new Color(155, 73, 80);
        public static Color SelectorPanelColor = new Color(103, 36, 31);

        public static Color InstructionTileColor = new Color(103, 36, 31);
        public static Color InstructionTileHighlightColor = new Color(31, 103, 83);
        public static Color InstructionTileAssignedVariableColor = Color.White;
        public static Color InstructionTileAssignedVariableHighlightColor = new Color(200, 255, 215);

        public static Color InstructionTileVariableColor = new Color(240, 240, 240);
        public static Color SelectionTileVariableColor = new Color(132, 126, 126);


    }
}
