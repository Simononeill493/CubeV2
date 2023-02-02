namespace CubeV2
{
    public abstract class ItemDroppingEntity: Entity
    {
        public ItemDroppingEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) { }

        public override void OnDestroy(Vector2Int formerLocation)
        {
            base.OnDestroy(formerLocation);

            EntityBoardCallback.TryCreate(GetDroppedItem(), formerLocation);
        }

        public abstract Entity GetDroppedItem();
    }
}
