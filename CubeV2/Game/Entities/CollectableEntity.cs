namespace CubeV2
{
    public class CollectableEntity : Entity
    {
        public CollectableEntity(string templateID,string ID, string sprite) : base(templateID,ID, sprite) { }

        public override bool TryBeCollected(Entity collector)
        {
            collector.CollectedEntities.Add(this);
            collector.Tags.Add(Config.CollectedGoalTag);
            return true;
        }
    }

}
