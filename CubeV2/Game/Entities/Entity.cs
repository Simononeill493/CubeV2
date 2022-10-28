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

            CurrentEnergy = Config.EntityMaxEnergy;
        }

        public void Tick(Board currentBoard)
        {
            var trueInstructionCount = 0;

            for (InstructionCounter = 0; InstructionCounter < Instructions.Count; InstructionCounter++)
            {
                var currentInstruction = Instructions[InstructionCounter];
                if(!HasEnergy(currentInstruction.BaseEnergyCost))
                {
                    continue;
                }

                var energyCost = currentInstruction.Run(this, currentBoard);

                CurrentEnergy -= energyCost;
                if(CurrentEnergy<0)
                {
                    Console.WriteLine("Entity energy is negative, which should never happen.");
                }
                
                for(int i=0;i<currentInstruction.OutputCount;i++)
                {
                    if (currentInstruction.OutputTargets[i] >= 0)
                    {
                        var output = currentInstruction.Outputs[i];
                        if(output!=null && output.DefaultType==IVariableType.StoredVariable)
                        {
                            //We can't store variable references in variables yet. causes overflow. too meta lol
                            continue;
                        }

                        Variables[currentInstruction.OutputTargets[i]] = output;
                    }
                }

                if(trueInstructionCount++ >= Config.MaxInstructionJumpsPerTick)
                {
                    break;
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

        public bool TryMove(RelativeDirection direction) => TryMove(DirectionUtils.ToCardinal(Orientation, direction));
        public void Hit(RelativeDirection direction) => Hit(DirectionUtils.ToCardinal(Orientation, direction));
        public CapturedTileVariable TryScan(RelativeDirection direction) => TryScan(DirectionUtils.ToCardinal(Orientation, direction));

        public bool TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            var didMoveWork = GameInterface.TryMoveEntity(this, newLocation);

            return didMoveWork;
        }

        public void Hit(CardinalDirection direction)
        {
            var targetLocation = Location + direction.XYOffset();
            GameInterface.ClearTile(targetLocation);
        }


        public CapturedTileVariable TryScan(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            var tile = GameInterface.TryGetTile(newLocation);

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
