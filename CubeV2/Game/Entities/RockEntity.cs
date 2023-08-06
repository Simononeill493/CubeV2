namespace CubeV2
{
    public class RockEntity : ItemDroppingEntity
    {
        public RockEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) { }

        public override Entity GetDroppedItem()
        {
            return EntityDatabase.Get(EntityDatabase.BrokenRockName).GenerateEntity();
        }
    }


}
