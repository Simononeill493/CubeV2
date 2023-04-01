using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Pipes;
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
        public const string VoidSprite = "Void";

        public const string Ally1Sprite = "Ally1";
        public const string Ally2Sprite = "Ally2";
        public const string Ally3Sprite = "Ally3";
        public const string Ally4Sprite = "Ally4";
        public const string Ally5Sprite = "Ally5";

        public const string CircuitGround1 = "Ground";
        public const string CircuitGround2 = "Ground2";

        public const string GrassGround = "GrassGround";

        public const string PlayerSprite = "Player";
        public const string RockSprite1 = "WhiteWall";
        public const string RockSprite2 = "RockSmall";

        public const string EnergyRockSprite = "EnergyRock";
        public const string BrokenRockSprite = "BrokenStone";

        public const string UpSprite = "UpArrow";
        public const string DownSprite = "DownArrow";
        public const string LeftSprite = "LeftArrow";
        public const string RightSprite = "RightArrow";
        public const string UpLeftSprite = "UpLeftArrow";
        public const string UpRightSprite = "UpRightArrow";
        public const string DownLeftSprite = "DownLeftArrow";
        public const string DownRightSprite = "DownRightArrow";

        public const string RainGif = "rain_TEST_ONLY-Sheet";

        public static Dictionary<string, Texture2D> SpritesDict;
        public static Dictionary<string, MyGif> GifDict;

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
            SpritesDict[VoidSprite] = content.Load<Texture2D>(VoidSprite);

            SpritesDict[Ally1Sprite] = content.Load<Texture2D>(Ally1Sprite);
            SpritesDict[Ally2Sprite] = content.Load<Texture2D>(Ally2Sprite);
            SpritesDict[Ally3Sprite] = content.Load<Texture2D>(Ally3Sprite);
            SpritesDict[Ally4Sprite] = content.Load<Texture2D>(Ally4Sprite);
            SpritesDict[Ally5Sprite] = content.Load<Texture2D>(Ally5Sprite);

            SpritesDict[CircuitGround1] = content.Load<Texture2D>(CircuitGround1);
            SpritesDict[CircuitGround2] = content.Load<Texture2D>(CircuitGround2);
            SpritesDict[GrassGround] = content.Load<Texture2D>(GrassGround);

            SpritesDict[PlayerSprite] = content.Load<Texture2D>(PlayerSprite);
            SpritesDict[RockSprite1] = content.Load<Texture2D>(RockSprite1);
            SpritesDict[RockSprite2] = content.Load<Texture2D>(RockSprite2);

            SpritesDict[BrokenRockSprite] = content.Load<Texture2D>(BrokenRockSprite);
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

            GifDict = new Dictionary<string, MyGif>();
            GifDict[RainGif] = new MyGif(content.Load<Texture2D>(RainGif), 200,200,30);

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

        public static int i;
        internal static void DrawGif(SpriteBatch spriteBatch, string gifName, Vector2 position, Vector2 scale, int rotation, Vector2 rotationOrigin, float layer,Color color)
        {
            var gif = GifDict[gifName];
            gif.Draw(spriteBatch,position,scale,rotation,rotationOrigin,layer,color);

        }


        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position, float scale, float rotation, Vector2 rotationOrigin, float layer, SpriteEffects flips = SpriteEffects.None) => DrawSprite(spriteBatch, spriteName, position, scale, rotation, rotationOrigin, layer, Color.White,flips);

        public static void DrawSprite(SpriteBatch spriteBatch, string spriteName, Vector2 position,float scale, float rotation, Vector2 rotationOrigin,float layer,Color color,SpriteEffects flips = SpriteEffects.None)
        {
            spriteBatch.Draw(SpritesDict[spriteName], position, null, color, rotation, rotationOrigin, scale, flips, layer);
        }

        public static void DrawString(SpriteBatch spriteBatch,SpriteFont font,string text,Vector2 position,Color color,float scale,float layer)
        {
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawRect(SpriteBatch spriteBatch,Vector2 position,Vector2 size,Color color,float layer)
        {
            spriteBatch.Draw(DefaultTexture, new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, 0, Vector2.Zero, SpriteEffects.None, layer);
        }


        public static void DrawTileSprite(SpriteBatch spriteBatch, string sprite, Orientation orientation, Vector2 position, float scale, float layer, SpriteEffects flips)
        {
            (float rotation, Vector2 rotationOffset) = _getRotationDataForOrientation(orientation);
            DrawSprite(spriteBatch, sprite, position + rotationOffset, scale, rotation, Vector2.Zero, layer,flips);
        }

        private static (float rotation,Vector2 rotationOffset) _getRotationDataForOrientation(Orientation orientation)
        {
            var rotBase = Math.PI / 4;
            float rotation = 0;
            Vector2 rotationOffset = Vector2.Zero;

            switch (orientation)
            {
                case Orientation.Top:
                    break;
                case Orientation.TopRight:
                    rotation = (float)(rotBase * 1);
                    rotationOffset = new Vector2(8, -4) * GameInterface.CameraScale;
                    break;
                case Orientation.Right:
                    rotation = (float)(rotBase * 2);
                    rotationOffset = new Vector2(16, 0) * GameInterface.CameraScale;

                    break;
                case Orientation.BottomRight:
                    rotation = (float)(rotBase * 3);
                    rotationOffset = new Vector2(19, 8) * GameInterface.CameraScale;

                    break;
                case Orientation.Bottom:
                    rotation = (float)(rotBase * 4);
                    rotationOffset = new Vector2(16, 16) * GameInterface.CameraScale;

                    break;
                case Orientation.BottomLeft:
                    rotation = (float)(rotBase * 5);
                    rotationOffset = new Vector2(8, 20) * GameInterface.CameraScale;

                    break;
                case Orientation.Left:
                    rotation = (float)(rotBase * 6);
                    rotationOffset = new Vector2(0, 16) * GameInterface.CameraScale;

                    break;
                case Orientation.TopLeft:
                    rotation = (float)(rotBase * 7);
                    rotationOffset = new Vector2(-3, 8) * GameInterface.CameraScale;
                    break;
                default:
                    throw new Exception();
            }

            return (rotation, rotationOffset);
        }

        public static string Sprite(this RelativeDirection dir) => _directionSpriteLookup[dir];
    }

    public class MyGif
    {
        public Texture2D SpriteSheet;
        public int Width;
        public int Height;
        public int NumFrames;

        public float frameCounter = 0;

        public MyGif(Texture2D spriteSheet,int width, int height, int numFrames)
        {
            SpriteSheet = spriteSheet;
            Width = width;
            Height = height;
            NumFrames = numFrames;
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, int rotation, Vector2 rotationOrigin, float layer,Color color)
        {
            if(frameCounter>=NumFrames)
            {
                frameCounter = 0;
            }

            var spriteRect = new Microsoft.Xna.Framework.Rectangle((int)(frameCounter) * Width, 0, Width, Height);
            spriteBatch.Draw(SpriteSheet, position, spriteRect, color, rotation, rotationOrigin, scale, SpriteEffects.None, layer);

            frameCounter+=0.4f;
        }
    }

}
