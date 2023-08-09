namespace CubeV2
{
    public abstract class ItemDroppingEntity: Entity
    {
        public ItemDroppingEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) { }

        public override void OnDoom(Board board,Vector2Int formerLocation)
        {
            base.OnDoom(board,formerLocation);

            board.TryAddEntityToBoard(GetDroppedItem(), formerLocation);
        }

        public abstract Entity GetDroppedItem();
    }
}
