using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class Board
    {
        public Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();

        public Dictionary<Vector2Int, Tile> TilesVector = new Dictionary<Vector2Int, Tile>();
        public List<Tile> TilesLinear = new List<Tile>();

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
                    entity.Tick();
                }
            }
        }

        public void TryMoveEntity(Entity entity, Vector2Int newLocation)
        {
            if (!TilesVector.ContainsKey(newLocation))
            {
                return;
            }

            var currentContents = TilesVector[newLocation].Contents;
            if (currentContents != null)
            {
                if (currentContents.TryBeCollected(entity))
                {
                    RemoveFromBoard(currentContents);
                }
                else
                {
                    return;
                }
            }

            RemoveFromBoard(entity);
            AddToBoard(entity, newLocation);
        }

        public void RemoveFromBoard(Entity entity)
        {
            if (!TilesVector.ContainsKey(entity.Location))
            {
                Console.WriteLine("Warning: Entity's current location is not valid for this board.");
            }

            if (TilesVector[entity.Location].Contents != entity)
            {
                Console.WriteLine("Warning: Entity is not in tile that its location data is pointing to.");
            }

            if (!Entities.ContainsKey(entity.Id))
            {
                Console.WriteLine("Warning: Entity is not in entity list that it is being removed from.");
            }


            TilesVector[entity.Location].Contents = null;
            entity.Location = Vector2Int.MinusOne;

            Entities.Remove(entity.Id);
        }

        public void AddToBoard(Entity entity, Vector2Int location)
        {
            if (entity.Location != Vector2Int.MinusOne)
            {
                Console.WriteLine("Warning: Entity already has a location");
            }

            if (TilesVector[location].Contents != null)
            {
                Console.WriteLine("Warning: Entity is being moved to tile which is not empty.");
            }

            if (Entities.ContainsKey(entity.Id))
            {
                Console.WriteLine("Warning: An entity with this ID already exists in this board.");
            }

            TilesVector[location].Contents = entity;
            entity.Location = location;

            Entities[entity.Id] = entity;
        }
    }
}
