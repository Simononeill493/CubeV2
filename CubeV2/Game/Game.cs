using CubeV2.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class Game
    {
        public bool GameWon => WinCondition.Check(CurrentBoard);
        public BoardWinCondition WinCondition = BoardWinCondition.None;

        public Board CurrentBoard { get; private set; }
        private BoardTemplate CurrentTemplate;
        private BoardTemplateTemplate CurrentTemplateTemplate;

        public void SetTemplateTemplate(BoardTemplateTemplate templateTemplate) => CurrentTemplateTemplate = templateTemplate;
        public void ResetBoardTemplate() => CurrentTemplate = CurrentTemplateTemplate.GenerateTemplate();
        public void ResetBoard() => SetBoard(CurrentTemplate.GenerateBoard());

        public void SetBoard(Board b) => CurrentBoard = b;
        public void TickBoard() => CurrentBoard.Tick();

        public List<Instruction> KnownInstructions = new List<Instruction>();

        public Game()
        {
            if (Config.KnowAllInstructionsByDefault)
            {
                KnownInstructions = InstructionDatabase.GetAll();
            }
        }
    }

    public abstract class BoardWinCondition
    {
        public abstract bool Check(Board board);

        public static BoardWinCondition None => new NoWinCondition();
    }

    public class NoWinCondition : BoardWinCondition
    {
        public override bool Check(Board board) => false;
    }

    public class GoalWinCondition : BoardWinCondition
    {
        string _toEnterGoalId;

        public GoalWinCondition(string toEnterGoalId)
        {
            _toEnterGoalId = toEnterGoalId;
        }

        public override bool Check(Board board)
        {
            return board.Entities[_toEnterGoalId].HasTag(Config.GoalTag);
        }

    }



    public class BoardTemplateTemplate
    {
        public int Width;
        public int Height;
        public List<EntityTemplate> Entities = new List<EntityTemplate>();

        public BoardTemplate GenerateTemplate()
        {
            var coords = Vector2Int.GetRandomUniqueCoords(Width, Height, Entities.Count);
            var entitiesDict = new Dictionary<Vector2Int, EntityTemplate>();

            for (int i = 0; i < Entities.Count; i++)
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
        public string Id { get; }
        public SpecialEntityTag SpecialTag;

        public string Sprite;
        public List<Instruction> Instructions = new List<Instruction>();

        public EntityTemplate(string id, SpecialEntityTag specialTag = SpecialEntityTag.None)
        {
            Id = id;
            SpecialTag = specialTag;
        }

        public Entity GenerateEntity()
        {
            switch (SpecialTag)
            {
                case SpecialEntityTag.None:
                    return new Entity(Id, Sprite) { Instructions = this.Instructions };
                case SpecialEntityTag.Goal:
                    return new GoalEntity(Id, Sprite) { Instructions = this.Instructions };
                default:
                    throw new Exception("Generating unrecognized entity type");
            }
        }

        public enum SpecialEntityTag
        {
            None,
            Goal
        }
    }


    public class Board
    {
        public Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();

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
                if (entity != null)
                {
                    entity.Tick();
                }
            }
        }

        public void TryMoveEntity(Entity entity, Vector2Int newLocation)
        {
            if (!TilesVector.ContainsKey(newLocation))
            {
                return;
            }

            var currentContents = TilesVector[newLocation].Contents;
            if (currentContents != null)
            {
                if (currentContents.TryBeCollected(entity))
                {
                    RemoveFromBoard(currentContents);
                }
                else
                {
                    return;
                }
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

            if (!Entities.ContainsKey(entity.Id))
            {
                Console.WriteLine("Warning: Entity is not in entity list that it is being removed from.");
            }


            TilesVector[entity.Location].Contents = null;
            entity.Location = Vector2Int.MinusOne;

            Entities.Remove(entity.Id);
        }

        public void AddToBoard(Entity entity, Vector2Int location)
        {
            if (entity.Location != Vector2Int.MinusOne)
            {
                Console.WriteLine("Warning: Entity already has a location");
            }

            if (TilesVector[location].Contents != null)
            {
                Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
            }

            if (Entities.ContainsKey(entity.Id))
            {
                Console.WriteLine("Warning: An entity with this ID already exists in this board.");
            }

            TilesVector[location].Contents = entity;
            entity.Location = location;

            Entities[entity.Id] = entity;
        }
    }


    public class Tile
    {
        public Entity Contents;
    }

    public class Entity
    {
        public string Id { get; }
        public string Sprite;

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;

        public List<Instruction> Instructions = new List<Instruction>();
        public int InstructionCounter;

        public List<string> Tags = new List<string>();
        public bool HasTag(string tag) => Tags.Contains(tag);

        public Entity(string id, string sprite)
        {
            Id = id;
            Sprite = sprite;
        }

        public void Tick()
        {
            for (int InstructionCounter = 0; InstructionCounter < Instructions.Count; InstructionCounter++)
            {
                Instructions[InstructionCounter].Run(this);
            }
        }

        public void TryMove(RelativeDirection direction) => TryMove(DirectionUtils.ToCardinal(Orientation, direction));

        public void TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            GameInterface.TryMoveEntity(this, newLocation);
        }

        public virtual bool TryBeCollected(Entity collector) => false;
    }

    public class GoalEntity : Entity
    {
        public GoalEntity(string id, string sprite) : base(id, sprite) { }

        public override bool TryBeCollected(Entity collector)
        {
            collector.Tags.Add(Config.GoalTag);
            return true;
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

    public static class InstructionDatabase
    {
        private static List<Instruction> _masterList;

        public static void Load()
        {
            _loadMasterList();
        }

        private static void _loadMasterList()
        {
            _masterList = new List<Instruction>();

            _masterList.Add(new MoveInstruction());
            _masterList.Add(new MoveRandomlyInstruction());

        }

        public static List<Instruction> GetAll()
        {
            var output = new List<Instruction>();
            foreach(var inst in _masterList)
            {
                output.Add(inst.GenerateNew());
            }
            return output;
        }
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

        public abstract Instruction GenerateNew();
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

            entity.TryMove((RelativeDirection)direction);
        }

        public override Instruction GenerateNew() => new MoveInstruction();
    }

    public class MoveRandomlyInstruction : Instruction
    {
        public override string Name => "Move Randomly";

        public override int VariableCount => 0;

        public override Instruction GenerateNew() => new MoveRandomlyInstruction();

        public override void Run(Entity entity)
        {
            entity.TryMove(RandomUtils.RandomRelative());
        }
    }
}
