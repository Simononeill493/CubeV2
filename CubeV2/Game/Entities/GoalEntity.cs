namespace CubeV2
{
    public class GoalEntity : CollectableEntity
    {
        public GoalEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) { }

        public override bool TryBeCollected(Entity collector)
        {
            if(base.TryBeCollected(collector))
            {
                collector.Tags.Add(Config.CollectedGoalTag);
                return true;
            }

            return false;
        }
    }
}
