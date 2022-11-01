namespace CubeV2
{
    public abstract class TileAppearance : Appearance
    {
        public TileAppearance(int index, float layer) : base(layer)
        {
            Index = index;
        }

        public int Index;
    }
}
