﻿using SAME;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class Board
    {
        public int Clock { get; private set; }

        private Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();
        private Dictionary<string, List<Entity>> EntityTypes = new Dictionary<string, List<Entity>>();
        public List<Entity> ActiveEntities = new List<Entity>();
        private List<Tile> TilesLinear = new List<Tile>();

        private Tile[,] TilesVector;

        public int _width { get; }
        public int _height { get; }
        public Vector2Int Size => new Vector2Int(_width, _height);

        public Board(int width, int height)
        {
            Clock = 0;

            _width = width;
            _height = height;

            TilesVector = new Tile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = new Tile();

                    TilesVector[x, y] = tile;
                    TilesLinear.Add(tile);
                }
            }

            /*foreach (var tile in TilesVector)
            {
                var adjacents = new List<Tile>();
                foreach (var adjPoint in tile.Key.GetAdjacentPoints())
                {
                    if (TilesVector.ContainsKey(adjPoint))
                    {
                        adjacents.Add(TilesVector[adjPoint]);
                    }
                }

                tile.Value.AdjacentTiles = adjacents;
            }
            */

        }

        public void Tick(UserInput input)
        {
            _executeEntityInstructions(input);
            _deleteDoomedEntities();

            Clock++;
        }

        private void _executeEntityInstructions(UserInput input)
        {
            var entities = ActiveEntities.ToList();
            foreach (var entity in entities)
            {
                if (entity != null && !entity.MarkedForDeletion && ((Clock % (entity.UpdateRate + entity.UpdateOffset) == 0)))
                {
                    entity.ExecuteInstructions(this, input);
                }
            }
        }



        private void _deleteDoomedEntities()
        {
            var toRemove = new List<Entity>();
            foreach (var entity in Entities.Values)
            {
                if (entity.MarkedForDeletion)
                {
                    toRemove.Add(entity);
                }
            }

            foreach (var e in toRemove)
            {
                _tryClearThisTile(e.Location);
                e.Deleted = true;
            }
        }

        public Tile TryGetTile(int index)
        {
            if (index > TilesLinear.Count)
            {
                Console.WriteLine("Warning: tried to fetch a tile with an index greater than the grid.");
                return null;
            }

            return TilesLinear[index];
        }
        public Tile TryGetTile(Vector2Int offset)
        {
            if (!ContainsLocation(offset))
            {
                return null;
            }

            return TilesVector[offset.X, offset.Y];
        }
        public bool TryDamageTile(Vector2Int offset, int amount)
        {
            if (ContainsLocation(offset))
            {
                TilesVector[offset.X, offset.Y].Damage(amount);
                return true;
            }

            return false;
        }


        public List<(Tile, Vector2Int)> GetSurroundings(Vector2Int location, int range)
        {
            var output = new List<(Tile, Vector2Int)>();
            for (int x = location.X - range; x <= location.X + range; x++)
            {
                for (int y = location.Y - range; y <= location.Y + range; y++)
                {
                    var searchLoc = new Vector2Int(x, y);
                    if (searchLoc == location)
                    {
                        continue;
                    }

                    var tile = TryGetTile(searchLoc);
                    if (tile != null)
                    {
                        output.Add((tile, searchLoc));
                    }
                }
            }

            return output;
        }

        public bool ContainsLocation(Vector2Int location)
        {
            return (location.X >= 0 && location.Y >= 0 && location.X < _width && location.Y < _height);
        }

        public bool TryMoveEntity(Entity entity, Vector2Int newLocation)
        {
            if (!ContainsLocation(newLocation))
            {
                return false;
            }

            var currentContents = TilesVector[newLocation.X, newLocation.Y].Contents;
            if (currentContents != null)
            {
                if (currentContents.TryBeCollected(entity))
                {
                    _removeFromBoard(currentContents);
                }
                else
                {
                    return false;
                }
            }

            RemoveEntityFromCurrentTile(entity);
            _tryAddEntityToTile(entity, newLocation);

            return true;
        }

        private bool _tryClearThisTile(Vector2Int targetLocation)
        {
            var tile = TryGetTile(targetLocation);
            if (tile != null)
            {
                if (tile.Contents != null)
                {
                    _removeFromBoard(tile.Contents);
                }
                return true;
            }

            return false;
        }

        private void _removeFromBoard(Entity entity)
        {
            var entityFormerLocation = entity.Location;

            RemoveEntityFromCurrentTile(entity);

            if (!Entities.ContainsKey(entity.EntityID))
            {
                Console.WriteLine("Warning: Entity is not in entity list that it is being removed from.");
            }

            Entities.Remove(entity.EntityID);
            _removeFromEntityTypesDict(entity);

            if (entity.IsActive)
            {
                _removeEntityFromActiveList(entity);
            }

            entity.MarkForDeletion();
            entity.WhenMarkedForDeletion(this, entityFormerLocation);
        }
        public void RemoveEntityFromCurrentTile(Entity entity)
        {
            if (!ContainsLocation(entity.Location))
            {
                Console.WriteLine("Warning: Entity's current location is not valid for this board.");
            }

            var tile = TilesVector[entity.Location.X, entity.Location.Y];
            if (tile.Contents != entity)
            {
                Console.WriteLine("Warning: Entity is not in tile that its location data is pointing to.");
            }

            tile.SetContents(null);
            entity.Location = Vector2Int.MinusOne;
        }

        public void AddEntityToBoard(Entity entity, int indexToAddTo) => TryAddEntityToBoard(entity, BoardUtils.IndexToXY(indexToAddTo, _width));

        public bool TryAddEntityToBoard(Entity entity, Vector2Int tileToAddTo)
        {
            if (!ContainsLocation(tileToAddTo))
            {
                return false;
            }

            if (Entities.ContainsKey(entity.EntityID))
            {
                //Console.WriteLine("Warning: An entity with this ID already exists in this board.");
                return false;
            }


            Entities[entity.EntityID] = entity;
            entity.CreationTime = Clock;

            if (!_tryAddEntityToTile(entity, tileToAddTo))
            {
                return false;
            }

            _addToEntityTypesDict(entity);

            if (entity.IsActive)
            {
                _addEntityToActiveList(entity);
            }

            return true;
        }


        private bool _tryAddEntityToTile(Entity entity, Vector2Int tileToAddTo)
        {
            if (entity.Location != Vector2Int.MinusOne)
            {
                //Console.WriteLine("Warning: Entity already has a location");
                return false;
            }

            if (TilesVector[tileToAddTo.X, tileToAddTo.Y].Contents != null)
            {
                //Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
                return false;
            }


            TilesVector[tileToAddTo.X, tileToAddTo.Y].SetContents(entity);
            entity.Location = tileToAddTo;
            return true;

        }

        private void _addToEntityTypesDict(Entity entity)
        {
            if (!EntityTypes.ContainsKey(entity.TemplateID))
            {
                EntityTypes[entity.TemplateID] = new List<Entity>();
            }

            if (EntityTypes[entity.TemplateID].Contains(entity))
            {
                Console.WriteLine("Warning: entity already exists in EntityTypes dict.");
                return;
            }

            EntityTypes[entity.TemplateID].Add(entity);
        }
        private void _removeFromEntityTypesDict(Entity entity)
        {
            if (!EntityTypes.ContainsKey(entity.TemplateID))
            {
                Console.WriteLine("Warning: TemplateID does not exist in EntityTypes dict.");
                return;
            }

            if (!EntityTypes[entity.TemplateID].Contains(entity))
            {
                Console.WriteLine("Warning: entity does not exist in EntityTypes dict that it's being removed from.");
                return;
            }

            EntityTypes[entity.TemplateID].Remove(entity);
        }

        private void _addEntityToActiveList(Entity entity)
        {
            if (ActiveEntities.Contains(entity))
            {
                Console.WriteLine("Warning: This entity already exists in the active list.");
            }
            else
            {
                ActiveEntities.Add(entity);
            }
        }
        private void _removeEntityFromActiveList(Entity entity)
        {
            if (!ActiveEntities.Contains(entity))
            {
                Console.WriteLine("Warning: This entity is not in the active list.");
            }
            else
            {
                ActiveEntities.Remove(entity);
            }
        }


        public List<Entity> GetEntityByTemplate(string templateID)
        {
            if (!EntityTypes.ContainsKey(templateID))
            {
                return new List<Entity>();
            }

            return EntityTypes[templateID];
        }

        public IEnumerable<Entity> GetActiveEntityByTag(string tag)
        {
            return ActiveEntities.Where(e => e.HasTag(tag));
        }

        public IEnumerable<Entity> GetEntityByTag(string tag)
        {
            return Entities.Values.Where(e => e.HasTag(tag));
        }
    }

    public static class BoardUtils
    {
        public static Vector2Int IndexToXY(int index, int width) => new Vector2Int(index % width, index / width);
        public static int XYToIndex(Vector2Int xy, int width) => xy.X + (xy.Y * width);

    }
}
