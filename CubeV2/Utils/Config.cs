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
        public const bool KnowAllInstructionsByDefault = true;
        public const bool InfiniteEnergy = true;


        public const bool EnableFogOfWar = false;
        public const bool EnableRangeLimits = false;
        public const bool EnablePlayerRangeOverlay = false;
        public const bool LockCameraMovement = true;
        public const bool LockZoomLevel = false;
        public const int DefaultZoomLevel = 3;


        public const string BoardTest1WorldPath = "C:\\Users\\Simon\\Desktop\\CubeV2\\EmptyMapSmall.txt";//30 14
        public const int BoardTest1StartingEnergy = 100;

        public const int EntityMaxInstructionsPerSet = 7;
        public const int EntityMaxVariables = 3;
        public const int GlobalDefaultMaxEnergy = 100;

        public const int ManualPlayerMaxEnergy = 200;
        public const int ManualPlayerStartingEnergy = 50;
        public const int PlayerOperationalRadius = 3;
        public const int PlayerVisualRadius = 7;
        public static Color PlayerFogColor = new Color(50, 50, 50);

        public const int BaseMoveCost = 1;//1
        public const int BaseCountSurroundingsCost = 2;//20
        public const int BaseScanCost = 3;//3
        public const int BaseHitCost = 5;//10
        public const int BasePingCost = 10;//20
        public const int BaseCreateCost = 100;//20

        public static TimeSpan DemoBoardUpdateRate = TimeSpan.FromSeconds(0.1);
        public static TimeSpan BoardTest1UpdateRate = TimeSpan.FromSeconds(0.075);

        //public const int GameGridWidth = 10;
        //public const int GameGridHeight = 10;
        //public const int GameGridHeight = 10;
        public const int GameUIGridMaxGridWidth = 100;
        public const int GameUIGridMaxGridHeight = 50;
        public const int GameUIGridPadding = 0;

        public static Vector2 TileBaseSize = new Vector2(16,16);
        public const int VariableSelectionTileScale = 3;
        public const int InstructionTileVariableScale = 3;


        public const int NumInstructionTiles = 7;
        public const int InstructionTileInternalPadding = 10;
        public const int InstructionTileTopPadding = 50;
        public const int InstructionMaxNumVariables = 3;
        public const int InstructionMaxNumOutputs = 2;
        public const int InstructionMaxNumControlOutputs = 2;
        public const int MaxInstructionJumpsPerTick = 12;




        public const string UITopLevelName = "UITopLevel";
        public const string InstructionPanelName = "InstructionPanel";
        public const string SelectorPanelName = "SelectorPanel";
        public const string InstructionSlotName = "InstructionSlot";
        public const string InstructionOutputTileName = "InstructionOutputTile";
        public const string InstructionControlOutputTileName = "InstructionControlOutputTile";
        public const string InstructionName = "Instruction";
        public const string AddInstructionButtonName = "AddInstructionButton";
        public const string RemoveInstructionButtonName = "RemoveInstructionButton";
        public const string RerollButtonName = "RerollButton";
        public const string GoButtonName = "GoButton";
        public const string ResetButtonName = "ResetButton";
        public const string SimulateButtonName = "SimulateButton";

        public const string GameGridName = "GameGrid";
        public const string CursorOverlayTileName = "CursorOverlayTile";
        public const string OperationalRangeOverlayTileName = "OperationalRangeOverlayTile";


        public const string ListOfInstructionsName = "InstructionSelectorGrid";
        public const string VariableCategoryListName = "VariableCategoryList";

        public const string InstructionVariableTileName = "InstructionVariableTile";
        public const string OutputSelectorGridName = "OutputSelectorGrid";

        public const string EnergyBarName = "EnergyBar";
        public const string DisplayTextName = "DisplayText";

        public const string CollectedGoalTag = "EnteredGoal";
        public const string PlayerTag = "Player";
        public const string IndestructibleTag = "Indestructible";

        public const string IntegerTextBoxName = "IntegerTextBox";

        public static void Load() 
        {
            _loadPositioning();
        }

        public static Vector2 ScreenSize = new Vector2(1900, 850);
        public static Vector2 GameUIGridMaxSize = new Vector2(1376, 704);


        public static Vector2 InstructionPanelSize;
        public static Vector2 InstructionPanelOffset;

        public static Vector2 SelectorPanelSize;
        public static Vector2 SelectorPanelOffset;

        public static Vector2 InstructionTileSize;
        public static Vector2 InstructionOptionTileSize = new Vector2(200, 40);
        public static Vector2 VariableCategoryTileSize = new Vector2(200, 40);

        public static Vector2 AddInstructionButtonOffset;
        public static Vector2 RemoveInstructionButtonOffset;
        public static int InstructionControlButtonsTopPadding = 15;

        public static Vector2Int ListOfInstructionsGridSize = new Vector2Int(1, 18);
        public static Vector2Int VariableCategoryListSize = new Vector2Int(1, 18);
        public static Vector2Int OutputSelectorGridSize = new Vector2Int(1, EntityMaxVariables);


        public static Vector2 GameControlButtonSize;
        public static Vector2 SimulateButtonSize;

        public static Vector2 GoButtonOffset;
        public static Vector2 ResetButtonOffset;
        public static Vector2 RerollButtonOffset;
        public static Vector2 SimulateButtonOffset;

        public static Vector2 EnergyBarSize;


        private static void _loadPositioning()
        {
            InstructionPanelSize = new Vector2(250, ScreenSize.Y);
            InstructionPanelOffset = Vector2.Zero;

            SelectorPanelSize = new Vector2(250, ScreenSize.Y);
            SelectorPanelOffset = new Vector2(Config.InstructionPanelSize.X, 0);

            InstructionTileSize = new Vector2(200, 100);

            AddInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.24f, InstructionControlButtonsTopPadding);
            RemoveInstructionButtonOffset = new Vector2(InstructionPanelSize.X * 0.6f, InstructionControlButtonsTopPadding);

            GameControlButtonSize = new Vector2(100, 60);
            SimulateButtonSize = new Vector2(200, 60);

            GoButtonOffset = new Vector2(740, 0);
            ResetButtonOffset = new Vector2(860,0);
            RerollButtonOffset = new Vector2(1000, 0);
            SimulateButtonOffset = new Vector2(1120, 0);

            EnergyBarSize = new Vector2(500, 20);

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
        public static Color SelectionTileVariableColor = new Color(162, 156, 156);

    }
}
