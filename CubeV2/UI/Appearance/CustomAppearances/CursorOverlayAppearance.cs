using CubeV2.Camera;
using Microsoft.Xna.Framework;

namespace CubeV2
{
    internal partial class UIBuilder
    {
        public class CursorOverlayAppearance : RectangleAppearance
        {
            public override Vector2 Size => GameCamera.TileSizeFloat;
            public CursorOverlayAppearance(Color color, float layer) : base(0, 0, color, layer){}
        }

    }
}
