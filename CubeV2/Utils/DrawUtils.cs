using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Color = Microsoft.Xna.Framework.Color;

namespace CubeV2.Utils
{
    internal class DrawUtils
    {

        public const string EnemySprite = "Enemy";
        public const string GoalSprite = "Goal";
        public const string GroundSprite = "Ground";
        public const string PlayerSprite = "Player";
        public const string WallSprite = "WhiteWall";

        public const string UpSprite = "UpArrow";
        public const string DownSprite = "DownArrow";
        public const string LeftSprite = "LeftArrow";
        public const string RightSprite = "RightArrow";
        public const string UpLeftSprite = "UpLeftArrow";
        public const string UpRightSprite = "UpRightArrow";
        public const string DownLeftSprite = "DownLeftArrow";
        public const string DownRightSprite = "DownRightArrow";

        public static Dictionary<string, Texture2D> SpritesDict;

        public const float BackgroundLayer = 1.0f;
        public const float UILayer1 = 0.19f;
        public const float UILayer2 = 0.18f;
        public const float UILayer3 = 0.17f;
        public const float UILayer4 = 0.16f;
        public const float UILayer5 = 0.15f;
        public const float OverlayLayer = 0.0f;

        public static Texture2D DefaultTexture;
        public static SpriteFont PressStart2PFont;

        public static void LoadContent(GraphicsDevice graphicsDevice,ContentManager content)
        {
            DefaultTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            DefaultTexture.SetData(new[] { Color.White });
            //Revisit this: this color is never used, but needs to be set for the texture to be visible??

            SpritesDict = new Dictionary<string, Texture2D>();
            SpritesDict[EnemySprite] = content.Load<Texture2D>(EnemySprite);
            SpritesDict[GoalSprite] = content.Load<Texture2D>(GoalSprite);
            SpritesDict[GroundSprite] = content.Load<Texture2D>(GroundSprite);
            SpritesDict[PlayerSprite] = content.Load<Texture2D>(PlayerSprite);
            SpritesDict[WallSprite] = content.Load<Texture2D>(WallSprite);

            SpritesDict[UpSprite] = content.Load<Texture2D>(UpSprite);
            SpritesDict[DownSprite] = content.Load<Texture2D>(DownSprite);
            SpritesDict[LeftSprite] = content.Load<Texture2D>(LeftSprite);
            SpritesDict[RightSprite] = content.Load<Texture2D>(RightSprite);
            SpritesDict[UpLeftSprite] = content.Load<Texture2D>(UpLeftSprite);
            SpritesDict[UpRightSprite] = content.Load<Texture2D>(UpRightSprite);
            SpritesDict[DownLeftSprite] = content.Load<Texture2D>(DownLeftSprite);
            SpritesDict[DownRightSprite] = content.Load<Texture2D>(DownRightSprite);

            PressStart2PFont = content.Load<SpriteFont>("PressStart2P");
        }

        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position, float scale, float layer) => DrawSprite(spriteBatch, spriteName, position, scale, layer, Color.White);

        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position,float scale,float layer,Color color)
        {
            spriteBatch.Draw(SpritesDict[spriteName], position, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawText(SpriteBatch spriteBatch,SpriteFont font,string text,Vector2 position,Color color,float layer)
        {
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, 1, SpriteEffects.None, layer);
        }

        public static string VariableToSprite(IVariable variable)
        {
            switch (variable.DefaultType)
            {
                case IVariableType.Direction:
                    return ((RelativeDirection)variable.Convert(IVariableType.Direction)).Sprite();
                default:
                    return PlayerSprite;
            }

        }
    }
}
