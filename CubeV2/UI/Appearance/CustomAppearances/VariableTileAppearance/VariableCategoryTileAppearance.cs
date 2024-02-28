using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SAME;

namespace CubeV2
{
    public class VariableCategoryAppearanceFactory : TileAppearanceFactory
    {
        private int _spriteScale;

        public VariableCategoryAppearanceFactory(int scale, float bg, float fg) : base(bg, fg)
        {
            _spriteScale = scale;
        }

        public override TileAppearance CreateForeground(int index)
        {
            return new VariableCategoryTileAppearance(index, _spriteScale, _foregroundLayer);
        }

        public override Appearance CreateBackground(int index, int width, int height)
        {
            var backgroundAppearance = new RectangleAppearance(width, height, Config.SelectionTileVariableColor, _backgroundLayer);
            backgroundAppearance.OverrideColor(() => (GameInterface.IsFocusedOnVariableOption(index) ? Config.InstructionTileAssignedVariableHighlightColor : Config.SelectionTileVariableColor));

            return backgroundAppearance;
        }

    }

    public class VariableCategoryTileAppearance : TileAppearance
    {
        private int _scale;
        public override Vector2 Size => Vector2.Zero;

        public VariableCategoryTileAppearance(int gridIndex, int scale, float layer) : base(gridIndex, layer)
        {
            _scale = scale;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            var variable = GetSource();
            if (variable != null)
            {
                spriteBatch.DrawString(DrawUtils.DefaultFont, variable.Name, position, Color.Black);
            }
        }


        public VariableCategory GetSource() => GameInterface.GetVariableCategory(Index);

    }

}
