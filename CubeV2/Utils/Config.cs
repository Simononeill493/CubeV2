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

        public static int DefaultScreenWidth = 1550;
        public static int DefaultScreenHeight = 800;

        public static int InstructionPanelWidth = 300;
        public static int SelectorPanelWidth = 300;

        public static int GameGridWidth = 8;
        public static int GameGridHeight = 6;
        public static int GameGridPadding = 4;

        public static int TileBaseSize = 16;
        public static int TileScale = 7;
        public static int VariableSelectionTileScale = 4;
        public static int InstructionTileVariableScale = 3;

        public static Vector2 InstructionSelectorTileSize = new Vector2(200, 75);
        public static Color InstructionSelectorTileColor = Color.White;

        public static Vector2 GameControlButtonSize = new Vector2(100, 60);
        public static Vector2 InstructionTileSize = new Vector2(200, 100);

        public static int NumInstructionTiles = 6;
        public static int InstructionTileInternalPadding = 10;
        public static int InstructionTileTopPadding = 50;
        public static int InstructionMaxNumVariables = 3;

        public static int InstructionControlButtonsTopPadding = 15;

        public static Color PrimaryBackgroundColor = new Color(80, 63, 63);
        public static Color TileBackgroundColor = new Color(32, 26, 26);
        public static Color InstructionPanelColor = new Color(155,73,80);
        public static Color SelectorPanelColor = new Color(103, 36, 31);

        public static Color InstructionTileColor = new Color(103, 36, 31);
        public static Color InstructionTileHighlightColor = new Color(31, 103, 83);
        public static Color InstructionTileAssignedVariableColor = Color.White;
        public static Color InstructionTileAssignedVariableHighlightColor = new Color(200,255,215);

        public static Color InstructionTileVariableColor = new Color(240, 240, 240);
        public static Color SelectionTileVariableColor = new Color(132, 126, 126);




        public static string UITopLevelName = "UITopLevel";
        public static string InstructionPanelName = "InstructionPanel";
        public static string SelectorPanelName = "SelectorPanel";
        public static string InstructionSlotName = "InstructionSlot";
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
        public static string WinTextName = "WinText";

        public static string GoalTag = "EnteredGoal";

        public static void Load() 
        {
        }
    }
}
