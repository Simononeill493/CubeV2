using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CubeV2
{
    public class Entity
    {
        public string TemplateID { get; }
        public string EntityID { get; }

        public string Sprite;

        public int MaxEnergy;
        public int CurrentEnergy;

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;

        public List<Instruction> Instructions = new List<Instruction>();
        public int InstructionCounter;

        public IVariable[] Variables = new IVariable[Config.InstructionMaxNumVariables];

        public List<string> Tags = new List<string>();
        public bool HasTag(string tag) => Tags.Contains(tag);

        public Entity(string templateID, string entityID, string sprite)
        {
            TemplateID = templateID;
            EntityID = entityID;
            Sprite = sprite;
        }

        public virtual void Tick(Board currentBoard, UserInput input)
        {
            var trueInstructionCount = 0;

            for (InstructionCounter = 0; InstructionCounter < Instructions.Count; InstructionCounter++)
            {
                _executeInstruction(Instructions[InstructionCounter], currentBoard);

                if(trueInstructionCount++ >= Config.MaxInstructionJumpsPerTick)
                {
                    break;
                }
            }
        }

        protected void _executeInstruction(Instruction currentInstruction,Board currentBoard)
        {
            if (!HasEnergy(currentInstruction.BaseEnergyCost))
            {
                return;
            }

            var energyCost = currentInstruction.Run(this, currentBoard);

            TakeEnergy(energyCost);

            //Set variable data
            for (int i = 0; i < currentInstruction.OutputCount; i++)
            {
                if (currentInstruction.OutputTargets[i] >= 0)
                {
                    var output = currentInstruction.Outputs[i];
                    if (output != null && output.DefaultType == IVariableType.StoredVariable)
                    {
                        //We can't store variable references in variables yet. causes overflow. too meta lol
                        continue;
                    }

                    Variables[currentInstruction.OutputTargets[i]] = output;
                }
            }

        }

        public void SetInstructionCounter(int index)
        {
            if(index>=0)
            {
                InstructionCounter = index;
            }
        }

        public bool HasEnergy(int amount)
        {
            return (CurrentEnergy >= amount);
        }

        public void TakeEnergy(int amount)
        {
            if (amount > CurrentEnergy)
            {
                throw new Exception("Taking more energy from an entity than it has. This should never happen.");
            }

            CurrentEnergy -= amount;
        }

        public void GiveEnergy(int amount)
        {
            CurrentEnergy += amount;

            if(CurrentEnergy>MaxEnergy)
            {
                CurrentEnergy = MaxEnergy;
            }
        }


        public bool TryMove(RelativeDirection direction) => TryMove(DirectionUtils.ToCardinal(Orientation, direction));
        public void Hit(RelativeDirection direction) => Hit(DirectionUtils.ToCardinal(Orientation, direction));
        public CapturedTileVariable TryScan(RelativeDirection direction) => TryScan(DirectionUtils.ToCardinal(Orientation, direction));

        public bool TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            var didMoveWork = BoardCallback.TryMoveEntity(this, newLocation);

            if(!didMoveWork && direction.IsDiagonal())
            {
                var diagonalIngredients = direction.GetDiagonalIngredients();

                var newLocationLeft = Location + diagonalIngredients.Left.XYOffset();
                didMoveWork = BoardCallback.TryMoveEntity(this, newLocationLeft);

                if(!didMoveWork)
                {
                    var newLocationRight = Location + diagonalIngredients.Right.XYOffset();
                    didMoveWork = BoardCallback.TryMoveEntity(this, newLocationRight);
                }
            }

            return didMoveWork;
        }

        public void Hit(CardinalDirection direction)
        {
            var targetLocation = Location + direction.XYOffset();
            BoardCallback.ClearTile(targetLocation);
        }


        public CapturedTileVariable TryScan(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            var tile = BoardCallback.TryGetTile(newLocation);

            if(tile!=null)
            {
                return new CapturedTileVariable(newLocation,tile.Contents);
            }

            return null;
        }




        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }

        public void SetOrientation(Orientation orientation)
        {
            Orientation = orientation;
        }


        public virtual bool TryBeCollected(Entity collector) => false;

    }
}
