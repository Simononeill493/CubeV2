using CubeV2.Camera;
using Microsoft.Xna.Framework;

namespace CubeV2
{
    internal partial class UIBuilder
    {
        public class OperationalRangeOverlayAppearance : RectangleAppearance
        {
            public override Vector2 Size => GameCamera.TileSizeFloat * ((Config.PlayerRangeLimit * 2 + 1));
            public OperationalRangeOverlayAppearance(Color color, float layer) : base(0, 0, color, layer) { }
        }

    }
}
