using System.Collections.Generic;
using System.Collections.Specialized;

namespace CubeV2
{
    public class Entity
    {
        public string Id { get; }
        public string Sprite;

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;

        public List<Instruction> Instructions = new List<Instruction>();
        public int InstructionCounter;

        public IVariable[] Variables = new IVariable[Config.InstructionMaxNumVariables];

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
                var currentInstruction = Instructions[InstructionCounter];
                currentInstruction.Run(this);
                
                for(int i=0;i<currentInstruction.OutputCount;i++)
                {
                    if (currentInstruction.OutputTargets[i] >= 0)
                    {
                        Variables[currentInstruction.OutputTargets[i]] = currentInstruction.Outputs[i];
                    }
                }
            }
        }

        public void TryMove(RelativeDirection direction) => TryMove(DirectionUtils.ToCardinal(Orientation, direction));

        public void TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            GameInterface.TryMoveEntity(this, newLocation);
        }

        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }

        public virtual bool TryBeCollected(Entity collector) => false;
    }
}
