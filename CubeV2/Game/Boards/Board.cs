using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class Board
    {
        private Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();
        private Dictionary<string, List<Entity>> EntityTypes = new Dictionary<string, List<Entity>>();

        private Dictionary<Vector2Int, Tile> TilesVector = new Dictionary<Vector2Int, Tile>();
        private List<Tile> TilesLinear = new List<Tile>();

        public Board(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var vector = new Vector2Int(x, y);
                    var tile = new Tile();

                    TilesVector[vector] = tile;
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

        public void Tick()
        {
            var entities = TilesVector.Values.Select(t => t.Contents).ToList();
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    entity.Tick(this);
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
            Tile tile;
            TilesVector.TryGetValue(offset, out tile);

            return tile;
        }

        public bool TryMoveEntity(Entity entity, Vector2Int newLocation)
        {
            if (!TilesVector.ContainsKey(newLocation))
            {
                return false;
            }

            var currentContents = TilesVector[newLocation].Contents;
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
        }
        public void RemoveEntityFromCurrentTile(Entity entity)
        {
            if (!TilesVector.ContainsKey(entity.Location))
            {
                Console.WriteLine("Warning: Entity's current location is not valid for this board.");
            }

            if (TilesVector[entity.Location].Contents != entity)
            {
                Console.WriteLine("Warning: Entity is not in tile that its location data is pointing to.");
            }

            TilesVector[entity.Location].Contents = null;
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
        }
        public void AddEntityToTile(Entity entity, Vector2Int tileToAddTo)
        {
            if (entity.Location != Vector2Int.MinusOne)
            {
                Console.WriteLine("Warning: Entity already has a location");
            }

            if (TilesVector[tileToAddTo].Contents != null)
            {
                Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
            }


            TilesVector[tileToAddTo].Contents = entity;
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

        public List<Entity> GetEntityByTemplate(string templateID)
        {
            if(!EntityTypes.ContainsKey(templateID))
            {
                return new List<Entity>();
            }

            return EntityTypes[templateID];
        }

    }
}
