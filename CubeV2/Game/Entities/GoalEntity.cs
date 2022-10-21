namespace CubeV2
{
    public class GoalEntity : Entity
    {
        public GoalEntity(string templateID,string ID, string sprite) : base(templateID,ID, sprite) { }

        public override bool TryBeCollected(Entity collector)
        {
            collector.Tags.Add(Config.GoalTag);
            return true;
        }
    }
}
