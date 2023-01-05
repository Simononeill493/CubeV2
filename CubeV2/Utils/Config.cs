using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
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

        public static int GlobalDefaultMaxEnergy = 100;
        public static int BoardTest1StartingEnergy = 100;

        public static int ManualPlayerMaxEnergy = 200;
        public static int ManualPlayerStartingEnergy = 50;
        public static int PlayerOperationalRadius = 3;

        public static int BaseMoveCost = 1;//1
        public static int BaseScanCost = 3;//3
        public static int BaseHitCost = 5;//10
        public static int BasePingCost = 10;//20

        public static TimeSpan DemoBoardUpdateRate = TimeSpan.FromSeconds(0.1);
        public static TimeSpan BoardTest1UpdateRate = TimeSpan.FromSeconds(0.075);

        //public static int GameGridWidth = 10;
        //public static int GameGridHeight = 10;
        public static int GameGridDefaultWidth = 43;
        public static int GameGridDefaultHeight = 22;
        public static int GameGridPadding = 0;

        public static Vector2 TileBaseSize = new Vector2(16,16);
        public static int TileScale = 2;
        public static int VariableSelectionTileScale = 4;
        public static int InstructionTileVariableScale = 3;

        public static int NumInstructionTiles = 6;
        public static int InstructionTileInternalPadding = 10;
        public static int InstructionTileTopPadding = 50;
        public static int InstructionMaxNumVariables = 3;
        public static int InstructionMaxNumOutputs = 2;
        public static int InstructionMaxNumControlOutputs = 2;
        public static int MaxInstructionJumpsPerTick = 12;




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
        public static string SimulateButtonName = "SimulateButton";

        public static string GameGridName = "GameGrid";
        public static string CursorOverlayTileName = "CursorOverlayTile";
        public static string OperationalRangeOverlayTileName = "OperationalRangeOverlayTile";


        public static string VariableGridName = "VariableGrid";
        public static string InstructionSelectorGridName = "InstructionSelectorGrid";
        public static string InstructionVariableTileName = "InstructionVariableTile";
        public static string OutputSelectorGridName = "OutputSelectorGrid";

        public static string EnergyBarName = "EnergyBar";
        public static string DisplayTextName = "DisplayText";

        public static string CollectedGoalTag = "EnteredGoal";
        public static string PlayerTag = "Player";

        public static void Load() 
        {
            _loadPositioning();
        }

        public static Vector2 ScreenSize = new Vector2(1900, 850);

        public static Vector2 InstructionPanelSize;
        public static Vector2 InstructionPanelOffset;

        public static Vector2 SelectorPanelSize;
        public static Vector2 SelectorPanelOffset;

        public static Vector2 InstructionTileSize;
        public static Vector2 InstructionOptionTileSize;

        public static Vector2 AddInstructionButtonOffset;
        public static Vector2 RemoveInstructionButtonOffset;
        public static int InstructionControlButtonsTopPadding = 15;

        public static Vector2Int VariableSelectorGridSize = new Vector2Int(4, 7);


        public static Vector2 GameControlButtonSize;
        public static Vector2 SimulateButtonSize;

        public static Vector2 GoButtonOffset;
        public static Vector2 ResetButtonOffset;
        public static Vector2 RerollButtonOffset;
        public static Vector2 SimulateButtonOffset;

        public static Vector2 EnergyBarSize;
        public static Vector2 EnergyBarOffset;
        public static Vector2 DisplayTextOffset;


        private static void _loadPositioning()
        {
            InstructionPanelSize = new Vector2(250, ScreenSize.Y);
            InstructionPanelOffset = Vector2.Zero;

            SelectorPanelSize = new Vector2(250, ScreenSize.Y);
            SelectorPanelOffset = new Vector2(Config.InstructionPanelSize.X, 0);

            InstructionTileSize = new Vector2(200, 100);
            InstructionOptionTileSize = new Vector2(200, 40);

            AddInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.24f, InstructionControlButtonsTopPadding);
            RemoveInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.6f, InstructionControlButtonsTopPadding);

            GameControlButtonSize = new Vector2(100, 60);
            SimulateButtonSize = new Vector2(200, 60);

            GoButtonOffset = new Vector2(0, 0);
            ResetButtonOffset = new Vector2(120,0);
            RerollButtonOffset = new Vector2(240, 0);
            SimulateButtonOffset = new Vector2(360, 0);

            EnergyBarSize = new Vector2(500, 20);
            EnergyBarOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 30, 30);
            DisplayTextOffset = new Vector2(InstructionPanelSize.X + SelectorPanelSize.X + 30, 8);

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
