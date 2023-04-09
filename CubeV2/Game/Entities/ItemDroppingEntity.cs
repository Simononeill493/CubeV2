namespace CubeV2
{
    public abstract class ItemDroppingEntity: Entity
    {
        public ItemDroppingEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) { }

        public override void OnDestroy(Board board,Vector2Int formerLocation)
        {
            base.OnDestroy(board,formerLocation);

            board.TryAddEntityToBoard(GetDroppedItem(), formerLocation);
        }

        public abstract Entity GetDroppedItem();
    }
}
