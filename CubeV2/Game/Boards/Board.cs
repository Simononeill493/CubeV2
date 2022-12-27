﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class Board
    {
        private Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();
        private Dictionary<string, List<Entity>> EntityTypes = new Dictionary<string, List<Entity>>();
        private List<Entity> ActiveEntities = new List<Entity>();
        private List<Tile> TilesLinear = new List<Tile>();

        private Tile[,] TilesVector;
        private int _width;
        private int _height;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;

            TilesVector = new Tile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = new Tile();

                    TilesVector[x,y] = tile;
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
            var entities = ActiveEntities.ToList();
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    entity.Tick(this,input);
                }
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
            if(!_onBoard(offset))
            {
                return null;
            }

            return TilesVector[offset.X, offset.Y];
        }

        private bool _onBoard(Vector2Int location)
        {
            return (location.X >= 0 && location.Y >= 0 && location.X < _width && location.Y < _height);
        }

        public bool TryMoveEntity(Entity entity, Vector2Int newLocation)
        {
            if (!_onBoard(newLocation))
            {
                return false;
            }

            var currentContents = TilesVector[newLocation.X,newLocation.Y].Contents;
            if (currentContents != null)
            {
                if (currentContents.TryBeCollected(entity))
                {
                    RemoveEntityFromBoard(currentContents);
                }
                else
                {
                    return false;
                }
            }

            RemoveEntityFromCurrentTile(entity);
            AddEntityToTile(entity, newLocation);

            return true;
        }

        internal void ClearThisTile(Vector2Int targetLocation)
        {
            var tile = TryGetTile(targetLocation);
            if(tile!=null && tile.Contents!=null)
            {
                RemoveEntityFromBoard(tile.Contents);
            }
        }


        public void RemoveEntityFromBoard(Entity entity)
        {
            RemoveEntityFromCurrentTile(entity);

            if (!Entities.ContainsKey(entity.EntityID))
            {
                Console.WriteLine("Warning: Entity is not in entity list that it is being removed from.");
            }

            Entities.Remove(entity.EntityID);
            _removeFromEntityTypesDict(entity);

            if (entity.Instructions != null)
            {
                _removeEntityFromActiveList(entity);
            }
        }
        public void RemoveEntityFromCurrentTile(Entity entity)
        {
            if (!_onBoard(entity.Location))
            {
                Console.WriteLine("Warning: Entity's current location is not valid for this board.");
            }

            if (TilesVector[entity.Location.X,entity.Location.Y].Contents != entity)
            {
                Console.WriteLine("Warning: Entity is not in tile that its location data is pointing to.");
            }

            TilesVector[entity.Location.X,entity.Location.Y].SetContents(null);
            entity.Location = Vector2Int.MinusOne;
        }

        public void AddEntityToBoard(Entity entity, Vector2Int tileToAddTo)
        {
            if (Entities.ContainsKey(entity.EntityID))
            {
                Console.WriteLine("Warning: An entity with this ID already exists in this board.");
            }


            Entities[entity.EntityID] = entity;

            AddEntityToTile(entity, tileToAddTo);
            _addToEntityTypesDict(entity);

            if(entity.Instructions!=null)
            {
                _addEntityToActiveList(entity);
            }
        }


        public void AddEntityToTile(Entity entity, Vector2Int tileToAddTo)
        {
            if (entity.Location != Vector2Int.MinusOne)
            {
                Console.WriteLine("Warning: Entity already has a location");
            }

            if (TilesVector[tileToAddTo.X,tileToAddTo.Y].Contents != null)
            {
                Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
            }


            TilesVector[tileToAddTo.X, tileToAddTo.Y].SetContents(entity);
            entity.Location = tileToAddTo;

        }

        private void _addToEntityTypesDict(Entity entity)
        {
            if(!EntityTypes.ContainsKey(entity.TemplateID))
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
            if(!EntityTypes.ContainsKey(templateID))
            {
                return new List<Entity>();
            }

            return EntityTypes[templateID];
        }

        public IEnumerable<Entity> GetActiveEntityByTag(string tag)
        {
            return ActiveEntities.Where(e => e.HasTag(tag));
        }
    }
}
