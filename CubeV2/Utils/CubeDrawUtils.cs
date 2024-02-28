using CubeV2.Camera;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
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
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SAME;

namespace CubeV2
{
    public static class CubeDrawUtils
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
        public const string FortressFloor = "FortressFloor";

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

        public const string MenuArrow1 = "MenuArrow1";


        public const string CraftingTableSprite = "Crafting_table";
        public const string MissileSprite = "Missile";
        public const string PickaxeFlowerSprite = "Pickaxe_flower";
        public const string RespawnerSprite = "Respawner";
        public const string StoneWallSprite = "Stone_wall";
        public const string TurretSprite = "Turret";
        public const string WeaponFlowerSprite = "Weapon_flower";


        public const string RainGif = "rain_TEST_ONLY-Sheet";
        public const string ExplosionGif = "testExplosion-Sheet";



        public static Dictionary<string, Texture2D> SpritesDict;
        public static Dictionary<string, CustomGifClass> GifDict;

        private static Dictionary<RelativeDirection, string> _directionSpriteLookup;
        //private static Dictionary<int, string> _numberSpriteLookup;

        public const float BackgroundLayer = 1.0f;

        public const float GameLayer1 = 0.29f;
        public const float GameLayer2 = 0.28f;
        public const float GameLayer3 = 0.27f;
        public const float GameLayer4 = 0.26f;
        public const float GameLayer5 = 0.25f;
        public const float GameLayer6 = 0.24f;
        public const float GameLayer7 = 0.23f;
        public const float GameLayer8 = 0.22f;
        public const float GameLayer9 = 0.21f;
        public const float BoardAnimationLayer = 0.209f;

        public const float UILayerBackground = 0.191f;
        public const float UILayer1 = 0.19f;
        public const float UILayer2 = 0.18f;
        public const float UILayer3 = 0.17f;
        public const float UILayer4 = 0.16f;
        public const float UILayer5 = 0.15f;
        public const float OverlayLayer = 0.0f;


        public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
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
            SpritesDict[FortressFloor] = content.Load<Texture2D>(FortressFloor);

            SpritesDict[PlayerSprite] = content.Load<Texture2D>(PlayerSprite);
            SpritesDict[RockSprite1] = content.Load<Texture2D>(RockSprite1);
            SpritesDict[RockSprite2] = content.Load<Texture2D>(RockSprite2);

            SpritesDict[BrokenRockSprite] = content.Load<Texture2D>(BrokenRockSprite);
            SpritesDict[EnergyRockSprite] = content.Load<Texture2D>(EnergyRockSprite);



            SpritesDict[CraftingTableSprite] = content.Load<Texture2D>(CraftingTableSprite);
            SpritesDict[MissileSprite] = content.Load<Texture2D>(MissileSprite);
            SpritesDict[PickaxeFlowerSprite] = content.Load<Texture2D>(PickaxeFlowerSprite);
            SpritesDict[RespawnerSprite] = content.Load<Texture2D>(RespawnerSprite);
            SpritesDict[StoneWallSprite] = content.Load<Texture2D>(StoneWallSprite);
            SpritesDict[TurretSprite] = content.Load<Texture2D>(TurretSprite);
            SpritesDict[WeaponFlowerSprite] = content.Load<Texture2D>(WeaponFlowerSprite);

            SpritesDict[UpSprite] = content.Load<Texture2D>(UpSprite);
            SpritesDict[DownSprite] = content.Load<Texture2D>(DownSprite);
            SpritesDict[LeftSprite] = content.Load<Texture2D>(LeftSprite);
            SpritesDict[RightSprite] = content.Load<Texture2D>(RightSprite);
            SpritesDict[UpLeftSprite] = content.Load<Texture2D>(UpLeftSprite);
            SpritesDict[UpRightSprite] = content.Load<Texture2D>(UpRightSprite);
            SpritesDict[DownLeftSprite] = content.Load<Texture2D>(DownLeftSprite);
            SpritesDict[DownRightSprite] = content.Load<Texture2D>(DownRightSprite);

            SpritesDict[MenuArrow1] = content.Load<Texture2D>(MenuArrow1);


            GifDict = new Dictionary<string, CustomGifClass>();
            GifDict[RainGif] = new CustomGifClass(content.Load<Texture2D>(RainGif), 200, 200, 30);
            GifDict[ExplosionGif] = new CustomGifClass(content.Load<Texture2D>(ExplosionGif), 16, 16, 14);

            _directionSpriteLookup = new Dictionary<RelativeDirection, string>
            {
                [RelativeDirection.Forward] = CubeDrawUtils.UpSprite,
                [RelativeDirection.Backward] = CubeDrawUtils.DownSprite,
                [RelativeDirection.Left] = CubeDrawUtils.LeftSprite,
                [RelativeDirection.Right] = CubeDrawUtils.RightSprite,
                [RelativeDirection.ForwardRight] = CubeDrawUtils.UpRightSprite,
                [RelativeDirection.BackwardRight] = CubeDrawUtils.DownRightSprite,
                [RelativeDirection.ForwardLeft] = CubeDrawUtils.UpLeftSprite,
                [RelativeDirection.BackwardLeft] = CubeDrawUtils.DownLeftSprite
            };
        }

        public static void DrawTileSprite(SpriteBatch spriteBatch, string sprite, Orientation orientation, Vector2 position, float scale, float layer, SpriteEffects flips)
        {
            (float rotation, Vector2 rotationOffset) = _getRotationDataForOrientation(orientation);
            DrawUtils.DrawSprite(spriteBatch, CubeDrawUtils.SpritesDict[sprite], position + rotationOffset, scale, rotation, Vector2.Zero, layer, flips);
        }

        internal static void DrawMeter(SpriteBatch spriteBatch, float percentage, Vector2 position, int cameraScale, float spriteMeterLayer, float spriteMeterLayer2)
        {
            var horizontalPadding = Config.TileBaseSize.X / 8;

            var widthBox = Config.TileBaseSize.X - (horizontalPadding * 2);
            var widthMeter = (float)Math.Ceiling(widthBox * percentage);

            var height = (Config.TileBaseSize.Y / 8);
            position.X += horizontalPadding * cameraScale;

            DrawUtils.DrawRect(spriteBatch, position, new Vector2(widthBox, height) * cameraScale, Color.Black, spriteMeterLayer);
            DrawUtils.DrawRect(spriteBatch, position, new Vector2(widthMeter, height) * cameraScale, Color.Red, spriteMeterLayer2);
        }


        private static (float rotation, Vector2 rotationOffset) _getRotationDataForOrientation(Orientation orientation)
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
                    rotationOffset = new Vector2(8, -4) * GameCamera.Scale;
                    break;
                case Orientation.Right:
                    rotation = (float)(rotBase * 2);
                    rotationOffset = new Vector2(16, 0) * GameCamera.Scale;

                    break;
                case Orientation.BottomRight:
                    rotation = (float)(rotBase * 3);
                    rotationOffset = new Vector2(19, 8) * GameCamera.Scale;

                    break;
                case Orientation.Bottom:
                    rotation = (float)(rotBase * 4);
                    rotationOffset = new Vector2(16, 16) * GameCamera.Scale;

                    break;
                case Orientation.BottomLeft:
                    rotation = (float)(rotBase * 5);
                    rotationOffset = new Vector2(8, 20) * GameCamera.Scale;

                    break;
                case Orientation.Left:
                    rotation = (float)(rotBase * 6);
                    rotationOffset = new Vector2(0, 16) * GameCamera.Scale;

                    break;
                case Orientation.TopLeft:
                    rotation = (float)(rotBase * 7);
                    rotationOffset = new Vector2(-3, 8) * GameCamera.Scale;
                    break;
                default:
                    throw new Exception();
            }

            return (rotation, rotationOffset);
        }

        public static string Sprite(this RelativeDirection dir) => _directionSpriteLookup[dir];

    }

    public class CustomGifClass
    {
        public Texture2D SpriteSheet;
        public int Width;
        public int Height;
        public int NumFrames;

        public CustomGifClass(Texture2D spriteSheet, int width, int height, int numFrames)
        {
            SpriteSheet = spriteSheet;
            Width = width;
            Height = height;
            NumFrames = numFrames;
        }

        internal void Draw(SpriteBatch spriteBatch, int frame, Vector2 position, Vector2 scale, int rotation, Vector2 rotationOrigin, float layer, Color color)
        {
            var spriteRect = new Rectangle(frame * Width, 0, Width, Height);
            spriteBatch.Draw(SpriteSheet, position, spriteRect, color, rotation, rotationOrigin, scale, SpriteEffects.None, layer);
        }
    }

}
