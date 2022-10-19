using CubeV2.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class Game
    {
        public void RerollBoard()
        {
            CurrentTemplate = CurrentTemplateTemplate.GenerateTemplate();
        }

        public void ResetBoard()
        {
            CurrentBoard = CurrentTemplate.GenerateBoard();
        }

        public void TickBoard()
        {
            CurrentBoard.Tick();

            GameWon = CurrentTemplateTemplate.WinCondition.Check(CurrentBoard);
        }

        public bool GameWon = false;

        public Board CurrentBoard;
        public BoardTemplate CurrentTemplate;
        public BoardTemplateTemplate CurrentTemplateTemplate;

    }

    public abstract class BoardWinCondition
    {
        public abstract bool Check(Board board);
    }

    public class BoardTemplateTemplate
    {
        public int Width;
        public int Height;
        public List<EntityTemplate> Entities = new List<EntityTemplate>();

        public BoardWinCondition WinCondition;

        public BoardTemplate GenerateTemplate()
        {
            var coords = Vector2Int.GetRandomUniqueCoords(Width, Height, Entities.Count);
            var entitiesDict = new Dictionary<Vector2Int, EntityTemplate>();

            for(int i=0;i<Entities.Count;i++)
            {
                entitiesDict[coords[i]] = Entities[i];
            }

            var template = new BoardTemplate();
            template.Width = Width;
            template.Height = Height;
            template.Entities = entitiesDict;

            return template;
        }
    }

    public class BoardTemplate
    {
        public Dictionary<Vector2Int, EntityTemplate> Entities = new Dictionary<Vector2Int, EntityTemplate>();
        public int Width;
        public int Height;

        public Board GenerateBoard()
        {
            var board = new Board(Width, Height);

            foreach (var entity in Entities)
            {
                board.AddToBoard(entity.Value.GenerateEntity(), entity.Key);
            }

            return board;
        }
    }

    public class EntityTemplate
    {
        public string Sprite;
        public List<Instruction> Instructions = new List<Instruction>();

        public Entity GenerateEntity()
        {
            return new Entity(Sprite) { Instructions = this.Instructions };
        }
    }


    public class Board
    {
        public Dictionary<Vector2Int, Tile> TilesVector = new Dictionary<Vector2Int, Tile>();
        public List<Tile> TilesLinear = new List<Tile>();

        public Board(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vector = new Vector2Int(x, y);
                    var tile = new Tile();

                    TilesVector[vector] = tile;
                    TilesLinear.Add(tile);
                }
            }

            /*foreach (var tile in TilesVector)
            {
                var adjacents = new List<Tile>();
                foreach (var adjPoint in tile.Key.GetAdjacentPoints())
                {
                    if (TilesVector.ContainsKey(adjPoint))
                    {
                        adjacents.Add(TilesVector[adjPoint]);
                    }
                }

                tile.Value.AdjacentTiles = adjacents;
            }
            */

        }

        public void Tick()
        {
            var entities = TilesVector.Values.Select(t => t.Contents).ToList();
            foreach (var entity in entities)
            {
                if(entity != null)
                {
                    entity.Tick();
                }
            }
        }
    
        public void TryMoveEntity(Entity entity,Vector2Int newLocation)
        {
            if(!TilesVector.ContainsKey(newLocation))
            {
                return;
            }

            if(TilesVector[newLocation].Contents != null)
            {
                return;
            }

            RemoveFromBoard(entity);
            AddToBoard(entity, newLocation);
        }

        public void RemoveFromBoard(Entity entity)
        {
            if (!TilesVector.ContainsKey(entity.Location))
            {
                Console.WriteLine("Warning: Entity's current location is not valid for this board.");
            }

            if (TilesVector[entity.Location].Contents != entity)
            {
                Console.WriteLine("Warning: Entity is not in tile that its location data is pointing to.");
            }

            TilesVector[entity.Location].Contents = null;
            entity.Location = Vector2Int.MinusOne;
        }

        public void AddToBoard(Entity entity, Vector2Int location)
        {
            if(entity.Location!=Vector2Int.MinusOne)
            {
                Console.WriteLine("Warning: Entity already has a location");
            }

            if (TilesVector[location].Contents != null)
            {
                Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
            }

            TilesVector[location].Contents = entity;
            entity.Location = location;

        }
    }


    public class Tile
    {
        public Entity Contents;
    }

    public class Entity
    {
        public string Sprite;

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;
        public List<Instruction> Instructions = new List<Instruction>();
        public int InstructionCounter;

        public Entity(string sprite)
        {
            Sprite = sprite;
        }

        public void Tick()
        {
            for(int InstructionCounter = 0; InstructionCounter < Instructions.Count; InstructionCounter++)
            {
                Instructions[InstructionCounter].Run(this);
            }
        }

        public void TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            GameInterface.TryMoveEntity(this, newLocation);
        }
        
    }

    public abstract class IVariable
    {
        public abstract IVariableType DefaultType { get; }
        public abstract List<IVariableType> ValidTypes { get; }

        public abstract object Convert(IVariableType variableType);
    }

    public static class VariableOptionsGenerator
    {
        public static List<IVariable> GetAllVariableOptions()
        {
            var options = new List<IVariable>();

            foreach (IVariableType variableType in typeof(IVariableType).GetEnumValues())
            {
                options.AddRange(GetVariableOptions(variableType));
            }

            return options;
        }

        public static List<IVariable> GetVariableOptions(IVariableType variableType)
        {
            var options = new List<IVariable>();

            switch (variableType)
            {
                case IVariableType.Direction:
                    foreach (var i in DirectionUtils.Relatives)
                    {
                        options.Add(new StaticDirectionVariable(i));
                    }
                    break;
                default:
                    throw new Exception("Make sure all variable types are handled");
            }

            return options;
        }

    }

    public class StaticDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.Direction;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.Direction };

        public RelativeDirection Direction { get; }

        public StaticDirectionVariable(RelativeDirection direction)
        {
            Direction = direction;
        }

        public override object Convert(IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.Direction:
                    return Direction;
                default:
                    return null;
            }
        }

    }

    /*public class RandomDirectionVariable : IVariable
    {
        public override object Convert(IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.Direction:
                    return RandomUtils.RandomRelative();
                default:
                    return null;
            }
        }

    }*/


    public enum IVariableType
    {
        Direction
    }

    public abstract class Instruction
    {
        public IVariable[] Variables;
        public abstract string Name { get; }
        public abstract int VariableCount { get; }

        public Instruction()
        {
            Variables = new IVariable[Config.InstructionMaxNumVariables];
            for(int i=0;i<Config.InstructionMaxNumVariables;i++)
            {
                Variables[i] = null;
            }
        }

        public abstract void Run(Entity entity);
    }

    public class MoveInstruction : Instruction
    {
        public override string Name => "Move";
        public override int VariableCount => 1;

        public MoveInstruction() : base() { }

        public MoveInstruction(RelativeDirection dir) : base()
        {
            Variables[0] = new StaticDirectionVariable(dir);
        }

        public override void Run(Entity entity)
        {
            var direction = Variables[0]?.Convert(IVariableType.Direction);
            if(direction==null)
            {
                return;
            }

            var directionToMove = DirectionUtils.ToCardinal(entity.Orientation, (RelativeDirection)direction);
            entity.TryMove(directionToMove);
        }
    }
}
