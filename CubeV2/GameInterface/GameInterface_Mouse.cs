using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public partial class GameInterface
    {
        private static void _processMouseActions(UserInput input)
        {
            if(input.ScrollDifference!=0)
            {
                ScrollWheelTurned(input.ScrollDirection);
            }
        }

        public static void ScrollWheelTurned(int scrollDirection)
        {
            if (scrollDirection > 0)
            {
                SelectedPlayerAction = PlayerActionUtils.RotateForward(SelectedPlayerAction);
            }
            else if (scrollDirection < 0)
            {
                SelectedPlayerAction = PlayerActionUtils.RotateBackwards(SelectedPlayerAction);
            }

        }

        public static void CurrentLeftClickAction(Tile tile, Vector2Int tileLocation,int index)
        {
            var distance = _game.FocusEntity.Location.EuclideanDistance(tileLocation);
            if(distance <= Config.PlayerOperationalRadius)
            {
                switch (SelectedPlayerAction)
                {
                    case PlayerActionSelection.Select:
                        FocusTile(index);
                        break;
                    case PlayerActionSelection.GiveEnergy:
                        FocusTile(index);
                        _manualGiveEmergy(tileLocation);
                        break;
                    case PlayerActionSelection.TakeEnergy:
                        FocusTile(index);
                        _manualTakeEmergy(tileLocation);
                        break;
                    case PlayerActionSelection.CreateEntity:
                        FocusTile(index);
                        _manualCreateEntity(tileLocation);
                        break;
                    case PlayerActionSelection.DestroyEntity:
                        _manualDestroyEntity(tileLocation);
                        break;
                }
            }

            return;
        }

        private static void _manualGiveEmergy(Vector2Int tileLocation)
        {
            var dropEnergy = new AdminDropEnergyInstruction();
            dropEnergy.Variables[0] = new LocationVariable(tileLocation);
            dropEnergy.Variables[1] = new IntegerVariable(10);

            ManualPlayerEntity.ClickInstruction = dropEnergy;
        }
        private static void _manualTakeEmergy(Vector2Int tileLocation)
        {
            var takeEnergy = new AdminTakeEnergyInstruction();
            takeEnergy.Variables[0] = new LocationVariable(tileLocation);
            takeEnergy.Variables[1] = new IntegerVariable(10);

            ManualPlayerEntity.ClickInstruction = takeEnergy;

        }
        private static void _manualCreateEntity(Vector2Int tileLocation)
        {
            var create = new AdminCreateInstruction();
            create.Variables[0] = new LocationVariable(tileLocation);
            create.Variables[1] = new EntityTypeVariable(EntityDatabase.GetTemplate(EntityDatabase.RockName));

            ManualPlayerEntity.ClickInstruction = create;
        }
        private static void _manualDestroyEntity(Vector2Int tileLocation)
        {
            var destroy = new AdminDestroyInstruction();
            destroy.Variables[0] = new LocationVariable(tileLocation);

            ManualPlayerEntity.ClickInstruction = destroy;
        }


        public static void CurrentRightClickAction(Tile tile, Vector2Int location,int index)
        {
            var distance = location.EuclideanDistance(_game.FocusEntity.Location);

            if (tile.Contents != null && distance <= Config.PlayerOperationalRadius)
            {
                _game.CurrentBoard.RemoveEntityFromBoard(tile.Contents);
            }
        }


        public static void LeftClickBoard(int index)
        {
            (var tile, var location) = _getBoardDetailsFromIndex(index);
            if (tile != null)
            {
                CurrentLeftClickAction(tile, location,index);
            }

        }
        public static void RightClickBoard(int index)
        {
            (var tile, var location) = _getBoardDetailsFromIndex(index);
            if(tile != null)
            {
                CurrentRightClickAction(tile, location,index);
            }
        }

        private static (Tile tile, Vector2Int Location) _getBoardDetailsFromIndex(int index)
        {
            var location = BoardUtils.IndexToXY(index, _game.CurrentBoard._width);
            var tile = _game.CurrentBoard.TryGetTile(location);

            if (tile != null)
            {
                return (tile, location);
            }

            return (null, Vector2Int.MinusOne);
        }


    }
}
