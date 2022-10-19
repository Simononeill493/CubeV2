using CubeV2.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CubeV2
{
    internal class GameInterface
    {
        public static List<IVariable> VariableOptions = new List<IVariable>();
        public static List<Instruction> FocusedInstructions;
        public static int FocusedInstruction;
        public static int FocusedVariable;

        public static void ChangeFocusedVariable(int optionIndex)
        {
            if(optionIndex < VariableOptions.Count)
            {
                var newVariable = VariableOptions[optionIndex];

                if(VariableExists(FocusedInstruction, FocusedVariable))
                {
                    FocusedInstructions[FocusedInstruction].Variables[FocusedVariable] = newVariable;
                }
            }
        }

        public static void FocusVariable(int instructionIndex, int variableIndex)
        {
            if(VariableExists(instructionIndex,variableIndex))
            {
                VariableOptions.Clear();
                VariableOptions.AddRange(VariableOptionsGenerator.GetAllVariableOptions());

                FocusedInstruction = instructionIndex;
                FocusedVariable = variableIndex;
            }
        }

        public static IVariable GetVariable(int instructionIndex,int variableIndex)=> FocusedInstructions[instructionIndex].Variables[variableIndex];


        public static bool VariableExists(int instructionIndex, int variableIndex)
        {
            if (InstructionExists(instructionIndex) && FocusedInstructions[instructionIndex].VariableCount > variableIndex)
            {
                return true;
            }

            return false;
        }
        public static bool InstructionExists(int instructionIndex)
        {
            if (FocusedInstructions != null && FocusedInstructions.Count > instructionIndex)
            {
                return true;
            }

            return false;

        }





        private static Game _game;

        private static bool BoardRunning;
        public static TimeSpan BoardUpdateRate;
        public static TimeSpan TimeSinceLastUpdate;



        public static void InitializeDemoGame()
        {
            _game = _createDemoGame();
            
        }
        public static void InitializeEmptyGame()
        {
            _game = new Game();
        }
        
        
        
        
        
        
        
        public static void ManualSetBoard(Board b)
        {
            _game.CurrentBoard = b;
        }

        public static void RerollBoard() => _game.RerollBoard();
        public static void ResetBoard() => _game.ResetBoard();

        public static void PauseBoard() => BoardRunning = false;
        public static void StartBoard(TimeSpan updateRate)
        {
            BoardRunning = true;
            BoardUpdateRate = updateRate;
            TimeSinceLastUpdate = TimeSpan.Zero;
        }

        public static void Update(GameTime gameTime)
        {
            if(BoardRunning)
            {
                TimeSinceLastUpdate += gameTime.ElapsedGameTime;
                if(TimeSinceLastUpdate>= BoardUpdateRate)
                {
                    TimeSinceLastUpdate = TimeSpan.Zero;
                    _game.TickBoard();
                }
            }
        }







        public static void TryMoveEntity(Entity e, Vector2Int location)
        {
            _game.CurrentBoard.TryMoveEntity(e, location);
        }

        public static Tile GetTile(int index)
        {
            return _game.CurrentBoard.TilesLinear[index];
        }

        public static void AddInstruction()
        {
            FocusedInstructions.Add(new MoveInstruction());
        }

        public static void RemoveInstruction()
        {
            if(FocusedInstructions.Any())
            {
                FocusedInstructions.RemoveAt(FocusedInstructions.Count - 1);
            }
        }



        private static Game _createDemoGame()
        {
            var game = new Game();
            var player = new EntityTemplate() { Sprite = DrawUtils.PlayerSprite };

            game.CurrentTemplateTemplate = _createDemoTemplateTemplate(player);
            game.RerollBoard();
            game.ResetBoard();

            FocusedInstructions = player.Instructions;

            return game;
        }

        private static BoardTemplateTemplate _createDemoTemplateTemplate(EntityTemplate player)
        {
            var templateTemplate = new BoardTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };

            player.Instructions = new List<Instruction>() { new MoveInstruction(RelativeDirection.Backward) };
            templateTemplate.Entities.Add(player);

            var wall = new EntityTemplate() { Sprite = DrawUtils.WallSprite };
            for (int i=0;i<10;i++)
            {
                templateTemplate.Entities.Add(wall);
            }

            templateTemplate.Entities.Add(new EntityTemplate() { Sprite = DrawUtils.GoalSprite });

            return templateTemplate;
        }

    }
}
