using System.Collections.Generic;

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
}
