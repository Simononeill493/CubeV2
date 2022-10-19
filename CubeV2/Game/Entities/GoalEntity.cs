namespace CubeV2
{
    public class GoalEntity : Entity
    {
        public GoalEntity(string id, string sprite) : base(id, sprite) { }

        public override bool TryBeCollected(Entity collector)
        {
            collector.Tags.Add(Config.GoalTag);
            return true;
        }
    }
}
