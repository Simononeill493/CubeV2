using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CubeV2
{
    internal class FortressTurorialGame : Game
    {
        private EntityTemplate _playerTemplate;

        public FortressTurorialGame(EntityTemplate player) : base()
        {
            _playerTemplate = player;
            SetFocusEntity((ManualPlayerEntity)_playerTemplate.GenerateEntity());

            SetTemplateTemplate(CreateTemplateTemplate());
            ResetBoardTemplate();
            ResetBoard();
        }

        public override void OnPlayerDeath()
        {
            RespawnPlayer();
        }

        public override void OnSetBoard(Board b)
        {
            GameInterface.DisplayText = "Fortress tutorial in development...";
            RespawnPlayer();
        }

        public override void RespawnPlayer()
        {
            FocusEntity.RestoreFromDeletion();
            FocusEntity.SetHealthToMax();
            FocusEntity.Location = Vector2Int.MinusOne;

            var spawner = CurrentBoard.GetEntityByTag(Config.SpawnerTag).First();
            var respawnLocation = spawner.Location + Vector2Int.Up;

            CurrentBoard.TryAddEntityToBoard(FocusEntity,respawnLocation);
        }


        public override BoardTemplateTemplate CreateTemplateTemplate()
        {
            var templateTemplate = _loadMapFromPNG(Config.FortressTutorialMapPath);
            _setGroundSpritesFromPNG(Config.FortressTutorialGroundSpritesPath, templateTemplate);
            return templateTemplate;
        }

        private FortressTutorialTemplateTemplate _loadMapFromPNG(string path)
        {
            var image = new Bitmap(path);
            var map = new FortressTutorialTemplateTemplate() { Width = image.Width, Height = image.Height };

            var blank = Color.FromArgb(0,0,0,0);

            var colorsToTemplates = new Dictionary<Color, EntityTemplate>();
            colorsToTemplates[Color.FromArgb(89, 89, 89)] = EntityDatabase.Get(EntityDatabase.StoneWallName);
            colorsToTemplates[Color.FromArgb(0, 0, 0)] = EntityDatabase.Get(EntityDatabase.RockName);
            colorsToTemplates[Color.FromArgb(255, 145, 0)] = EntityDatabase.Get(EntityDatabase.RespawnerName);
            colorsToTemplates[Color.FromArgb(221, 126, 0)] = EntityDatabase.Get(EntityDatabase.CraftingTableName);
            colorsToTemplates[Color.FromArgb(157, 0, 255)] = EntityDatabase.Get(EntityDatabase.TurretName);
            colorsToTemplates[Color.FromArgb(255, 251, 0)] = EntityDatabase.Get(EntityDatabase.GoalName);
            colorsToTemplates[Color.FromArgb(0, 97, 255)] = EntityDatabase.Get(EntityDatabase.LaserFlowerName);
            colorsToTemplates[Color.FromArgb(255, 103, 0)] = EntityDatabase.Get(EntityDatabase.PickaxeFlowerName);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var tileColor = image.GetPixel(x, y);
                    if (colorsToTemplates.ContainsKey(tileColor))
                    {
                        map.StaticEntities[new Vector2Int(x, y)] = colorsToTemplates[tileColor];
                    }
                    else if (!tileColor.Equals(blank))
                    {
                        Console.WriteLine("Unknown color in map file. Position: {" + x + " " + y + "}");
                    }
                }
            }
            return map;
        }

        private void _setGroundSpritesFromPNG(string path,FortressTutorialTemplateTemplate map)
        {
            var image = new Bitmap(path);

            if(image.Width!= map.Width || image.Height!= map.Height)
            {
                throw new Exception("Ground sprite image dimensions are {" + image.Width + " " + image.Height + "}. Required dimensions are {" + map.Width + " " + map.Height + "}.");
            }

            var colorsToSprites = new Dictionary<Color, string>();
            colorsToSprites[Color.FromArgb(172,172,172)] = DrawUtils.FortressFloor;
            colorsToSprites[Color.FromArgb(255,255,255)] = DrawUtils.GrassGround;

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var tileColor = image.GetPixel(x, y);
                    if (colorsToSprites.ContainsKey(tileColor))
                    {
                        map.GroundSprites[new Vector2Int(x, y)] = colorsToSprites[tileColor];
                    }
                    else
                    {
                        map.GroundSprites[new Vector2Int(x, y)] = DrawUtils.VoidSprite;
                    }
                }
            }
        }

    }
}
