using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Color = Microsoft.Xna.Framework.Color;

namespace CubeV2
{
    public static class DrawUtils
    {
        public const string EnemySprite = "Enemy";
        public const string GoalSprite = "Goal";
        public const string PortalSprite = "Portal";

        public const string GroundSprite = "Ground";
        public const string PlayerSprite = "Player";
        public const string RockSprite = "WhiteWall";
        public const string EnergyRockSprite = "EnergyRock";

        public const string UpSprite = "UpArrow";
        public const string DownSprite = "DownArrow";
        public const string LeftSprite = "LeftArrow";
        public const string RightSprite = "RightArrow";
        public const string UpLeftSprite = "UpLeftArrow";
        public const string UpRightSprite = "UpRightArrow";
        public const string DownLeftSprite = "DownLeftArrow";
        public const string DownRightSprite = "DownRightArrow";

        public static Dictionary<string, Texture2D> SpritesDict;
        private static Dictionary<RelativeDirection, string> _directionSpriteLookup;
        //private static Dictionary<int, string> _numberSpriteLookup;

        public const float BackgroundLayer = 1.0f;

        public const float GameLayer1 = 0.29f;
        public const float GameLayer2 = 0.28f;
        public const float GameLayer3 = 0.27f;
        public const float GameLayer4 = 0.26f;
        public const float GameLayer5 = 0.25f;

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
            SpritesDict[PortalSprite] = content.Load<Texture2D>(PortalSprite);

            SpritesDict[GroundSprite] = content.Load<Texture2D>(GroundSprite);
            SpritesDict[PlayerSprite] = content.Load<Texture2D>(PlayerSprite);
            SpritesDict[RockSprite] = content.Load<Texture2D>(RockSprite);
            SpritesDict[EnergyRockSprite] = content.Load<Texture2D>(EnergyRockSprite);

            SpritesDict[UpSprite] = content.Load<Texture2D>(UpSprite);
            SpritesDict[DownSprite] = content.Load<Texture2D>(DownSprite);
            SpritesDict[LeftSprite] = content.Load<Texture2D>(LeftSprite);
            SpritesDict[RightSprite] = content.Load<Texture2D>(RightSprite);
            SpritesDict[UpLeftSprite] = content.Load<Texture2D>(UpLeftSprite);
            SpritesDict[UpRightSprite] = content.Load<Texture2D>(UpRightSprite);
            SpritesDict[DownLeftSprite] = content.Load<Texture2D>(DownLeftSprite);
            SpritesDict[DownRightSprite] = content.Load<Texture2D>(DownRightSprite);

            PressStart2PFont = content.Load<SpriteFont>("PressStart2P");


            _directionSpriteLookup = new Dictionary<RelativeDirection, string>
            {
                [RelativeDirection.Forward] = DrawUtils.UpSprite,
                [RelativeDirection.Backward] = DrawUtils.DownSprite,
                [RelativeDirection.Left] = DrawUtils.LeftSprite,
                [RelativeDirection.Right] = DrawUtils.RightSprite,
                [RelativeDirection.ForwardRight] = DrawUtils.UpRightSprite,
                [RelativeDirection.BackwardRight] = DrawUtils.DownRightSprite,
                [RelativeDirection.ForwardLeft] = DrawUtils.UpLeftSprite,
                [RelativeDirection.BackwardLeft] = DrawUtils.DownLeftSprite
            };
        }

        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position, float scale, float rotation, Vector2 rotationOrigin, float layer) => DrawSprite(spriteBatch, spriteName, position, scale, rotation, rotationOrigin, layer, Color.White);

        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position,float scale, float rotation, Vector2 rotationOrigin,float layer,Color color)
        {
            spriteBatch.Draw(SpritesDict[spriteName], position, null, color, rotation, rotationOrigin, scale, SpriteEffects.None, layer);
        }

        public static void DrawString(SpriteBatch spriteBatch,SpriteFont font,string text,Vector2 position,Color color,float scale,float layer)
        {
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawRect(SpriteBatch spriteBatch,Vector2 position,Vector2 size,Color color,float layer)
        {
            spriteBatch.Draw(DefaultTexture, new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, 0, Vector2.Zero, SpriteEffects.None, layer);
        }

        public static void DrawEntity(SpriteBatch spriteBatch, Entity entity, Vector2 position, float scale, float layer)
        {
            var rotBase = Math.PI / 4;
            float rotation = 0;
            Vector2 rotationOffset = Vector2.Zero;

            switch (entity.Orientation)
            {
                case Orientation.Top:
                    break;
                case Orientation.TopRight:
                    rotation = (float)(rotBase * 1);
                    rotationOffset = new Vector2(8, -4) * Config.TileScale;
                    break;
                case Orientation.Right:
                    rotation = (float)(rotBase*2);
                    rotationOffset = new Vector2(16, 0) * Config.TileScale;

                    break;
                case Orientation.BottomRight:
                    rotation = (float)(rotBase * 3);
                    rotationOffset = new Vector2(19, 8) * Config.TileScale;

                    break;
                case Orientation.Bottom:
                    rotation = (float)(rotBase * 4);
                    rotationOffset = new Vector2(16, 16) * Config.TileScale;

                    break;
                case Orientation.BottomLeft:
                    rotation = (float)(rotBase * 5);
                    rotationOffset = new Vector2(8, 20) * Config.TileScale;

                    break;
                case Orientation.Left:
                    rotation = (float)(rotBase * 6);
                    rotationOffset = new Vector2(0, 16) * Config.TileScale;

                    break;
                case Orientation.TopLeft:
                    rotation = (float)(rotBase * 7);
                    rotationOffset = new Vector2(-3, 8) * Config.TileScale;
                    break;    
                default:
                    throw new Exception();
            }

            DrawSprite(spriteBatch, entity.Sprite, position + rotationOffset, scale, rotation, Vector2.Zero, layer);
        }

        public static string Sprite(this RelativeDirection dir) => _directionSpriteLookup[dir];
    }
}
