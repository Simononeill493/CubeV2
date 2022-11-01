namespace CubeV2
{
    public class Tile
    {
        public Entity Contents { get; private set; }

        public void SetContents(Entity e)
        {
            Contents = e;
        }
    }
}
