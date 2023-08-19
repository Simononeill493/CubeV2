using System;

namespace CubeV2
{
    public abstract class ItemDroppingEntity: Entity
    {
        public ItemDroppingEntity(string templateID, string ID, string sprite) : base(templateID, ID, sprite) 
        {
            throw new NotImplementedException();
            //uses the old logic for destroying items. needs update (also I don't know if I'm even going to use this)
        }

        public override void WhenMarkedForDeletion(Board board,Vector2Int formerLocation)
        {
            base.WhenMarkedForDeletion(board,formerLocation);

            board.TryAddEntityToBoard(GetDroppedItem(), formerLocation);
        }

        public abstract Entity GetDroppedItem();
    }
}
