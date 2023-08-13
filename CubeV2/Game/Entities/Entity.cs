using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CubeV2
{
    public class Entity
    {
        public bool Doomed = false;

        public string TemplateID { get; }
        public string EntityID { get; }
        public string GetEntityName() => TemplateID + "_" + EntityID;

        public string Sprite;

        public int UpdateRate = 1;
        public int CreationTime;

        public int MaxEnergy;
        public int GetCurrentEnergy() => Config.InfiniteEnergy ? MaxEnergy : _currentEnergy;

        private int _currentEnergy;
        public string GetEnergyOverMaxAsText() => Config.InfiniteEnergy ? "INF" : GetCurrentEnergy() + "/" + MaxEnergy;
        public float GetEnergyPercentage() => (MaxEnergy == 0 || Config.InfiniteEnergy) ? 1 : GetCurrentEnergy() / MaxEnergy;

        public bool ShowHarvestMeter = false;
        protected int _maxHarvestCount;
        protected int _currentHarvestCount;
        public float GetHarvestPercentage() => (_maxHarvestCount == 0 ) ? 1 : (((float)_currentHarvestCount) / _maxHarvestCount);

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;

        public bool IsActive => Instructions?[0] != null;
        public List<Instruction[]> Instructions = new List<Instruction[]>();
        public int CurrentInstructionSet = 0;
        public int InstructionCounter;

        public IVariable[] InstructionOutputs;
        public IVariable[] Variables = new IVariable[Config.InstructionMaxNumVariables];

        public List<string> Tags = new List<string>();
        public bool HasTag(string tag) => Tags.Contains(tag);

        public List<CollectableEntity> CollectedEntities = new List<CollectableEntity>();

        public Entity(string templateID, string entityID, string sprite)
        {
            TemplateID = templateID;
            EntityID = entityID;
            Sprite = sprite;

            Instructions.Add(new Instruction[Config.EntityMaxInstructionsPerSet]);

            InstructionOutputs = new IVariable[Config.InstructionMaxNumOutputs];
            for (int i = 0; i < Config.InstructionMaxNumOutputs; i++)
            {
                InstructionOutputs[i] = null;
            }
        }

        public virtual void ExecuteInstructions(Board currentBoard, UserInput input)
        {
            var trueInstructionCount = 0;

            for (InstructionCounter = 0; InstructionCounter < Config.EntityMaxInstructionsPerSet; InstructionCounter++)
            {
                var instruction = Instructions[CurrentInstructionSet][InstructionCounter];
                if (instruction != null)
                {
                    _executeInstruction(instruction, currentBoard);
                }

                if (trueInstructionCount++ >= Config.MaxInstructionJumpsPerTick || Doomed)
                {
                    break;
                }
            }
        }

        protected void _executeInstruction(Instruction currentInstruction, Board currentBoard)
        {
            if (!HasEnergy(currentInstruction.BaseEnergyCost))
            {
                return;
            }

            var energyCost = currentInstruction.Run(this, currentBoard);
            if(currentInstruction.ControlFlowOutputs[0] >= 0 && currentInstruction.ControlOutputCount==1)
            {
                SetInstructionCounter(currentInstruction.ControlFlowOutputs[0] - 1);
            }

            TakeEnergy(energyCost);

            //Set variable data
            for (int i = 0; i < currentInstruction.OutputCount; i++)
            {
                if (currentInstruction.OutputTargetVariables[i] >= 0)
                {
                    var output = InstructionOutputs[i];
                    if (output != null && output.DefaultType == IVariableType.StoredVariable)
                    {
                        //We can't store variable references in variables yet. causes overflow. too meta lol
                        continue;
                    }

                    Variables[currentInstruction.OutputTargetVariables[i]] = output;
                }
            }

        }

        public void SetInstructionCounter(int index)
        {
            if (index >= 0)
            {
                InstructionCounter = index;
            }
        }

        public bool HasEnergy(int amount)
        {
            return (Config.InfiniteEnergy) || (_currentEnergy >= amount);
        }
        public void TakeEnergy(int amount)
        {
            if (amount > _currentEnergy && !Config.InfiniteEnergy)
            {
                throw new Exception("Taking more energy from an entity than it has. This should never happen.");
            }

            _currentEnergy -= amount;
        }
        public void GiveEnergy(int amount)
        {
            _currentEnergy += amount;

            if (_currentEnergy > MaxEnergy)
            {
                _currentEnergy = MaxEnergy;
            }
        }
        public void SetEnergyToMax() => _currentEnergy = MaxEnergy;

        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }
        public void SetOrientation(Orientation orientation)
        {
            Orientation = orientation;
        }

        public bool TrySwitchInstructionSet(int index)
        {
            if (index >=0 && index < Instructions.Count)
            {
                CurrentInstructionSet = index;
                return true;
            }

            return false;
        }


        public bool TryMove(Board board, RelativeDirection direction) => TryMove(board,DirectionUtils.ToCardinal(Orientation, direction));
        public bool TryPushEnergy(Board board, RelativeDirection direction, int amount) => TryPushEnergy(board, DirectionUtils.ToCardinal(Orientation, direction), amount);
        public bool TryPullEnergy(Board board, RelativeDirection direction) => TryPullEnergy(board, DirectionUtils.ToCardinal(Orientation, direction));
        public CapturedTileVariable TryPushScan(Board board, RelativeDirection direction) => TryPushScan(board, DirectionUtils.ToCardinal(Orientation, direction));
        public void PushDestroy(Board board, RelativeDirection direction) => PushDestroy(board, DirectionUtils.ToCardinal(Orientation, direction));

        public bool TryMove(Board board,CardinalDirection direction)
        {
            var approachVector = direction.ToVector();
            var newLocation = Location + approachVector;

            var didMoveWork = board.TryMoveEntity(this, newLocation);
            if(didMoveWork)
            {
                AnimationTracker.AddEntityMovement(EntityID, UpdateRate, approachVector);
            }

            /* Code to 'flow' around blocks when moving diagonaally.
             * Causes unintuitive behaviour when programming blocks, but we can go back to it later.
            if (!didMoveWork && direction.IsDiagonal())
            //if (!didMoveWork)
            {
                var adjacentDirections = direction.GetAdjacentDirections();

                var newLocationLeft = Location + adjacentDirections.Left.ToVector();
                didMoveWork = EntityBoardCallback.TryMove(this, newLocationLeft);

                if (!didMoveWork)
                {
                    var newLocationRight = Location + adjacentDirections.Right.ToVector();
                    didMoveWork = EntityBoardCallback.TryMove(this, newLocationRight);
                }
            }*/

            return didMoveWork;
        }

        public bool TryPushEnergy(Board board, CardinalDirection direction, int amount)
        {
            var newLocation = Location + direction.ToVector();
            var target = board.TryGetTile(newLocation);

            return TryDropEnergy(target, amount);
        }
        public bool TryDropEnergy(Tile target, int amount)
        {
            if (_currentEnergy < amount && !Config.InfiniteEnergy)
            {
                amount = _currentEnergy;
            }

            if (target == null || target.Contents == null || amount == 0 || (target.Contents.HasEnergy(target.Contents.MaxEnergy)))
            {
                return false;
            }


            TakeEnergy(amount);
            target.Contents.GiveEnergy(amount);

            return true;
        }

        public bool TryPullEnergy(Board board, CardinalDirection direction)
        {
            var newLocation = Location + direction.ToVector();
            var target = board.TryGetTile(newLocation);

            return TryTakeEnergy(target, 1);
        }
        public bool TryTakeEnergy(Tile target, int amount)
        {
            if (target == null || target.Contents == null || _currentEnergy >= MaxEnergy || Config.InfiniteEnergy)
            {
                return false;
            }

            if (!target.Contents.HasEnergy(amount))
            {
                amount = target.Contents._currentEnergy;

                if (amount < 1)
                {
                    return false;
                }
            }

            target.Contents.TakeEnergy(amount);
            GiveEnergy(amount);
            return true;
        }

        public CapturedTileVariable TryPushScan(Board board, CardinalDirection direction)
        {
            var newLocation = Location + direction.ToVector();
            var tile = board.TryGetTile(newLocation);

            if (tile != null)
            {
                return new CapturedTileVariable(newLocation, tile.Contents);
            }

            return null;
        }

        public void PushDestroy(Board board, CardinalDirection direction)
        {
            var targetLocation = Location + direction.ToVector();
            board.TryClearThisTile(targetLocation);
        }

        public virtual void OnDoom(Board board,Vector2Int formerLocation) {}
        public virtual bool TryBeCollected(Entity collector) => false;
        public virtual bool TryLeftPress() => false;
    }
}
