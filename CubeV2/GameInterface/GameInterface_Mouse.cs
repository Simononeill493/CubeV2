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
        public static void TryLeftClickTile(int uiIndex)
        {
            (var tile, var location, var actualIndex) = _getBoardDetailsFromIndex(uiIndex);
            if (tile != null)
            {
                LeftClickTile(tile, location, actualIndex);
            }
        }

        public static void TryRightClickTile(int uiIndex)
        {
            (var tile, var location, var actualIndex) = _getBoardDetailsFromIndex(uiIndex);
            if (tile != null)
            {
                RightClickTile(tile, location, actualIndex);
            }
        }

        public static void TryLeftPressTile(int uiIndex)
        {
            (var tile, var location, var actualIndex) = _getBoardDetailsFromIndex(uiIndex);
            if (tile != null)
            {
                LeftPressTile(tile, location, actualIndex);
            }
        }

        public static void TryRightPressTile(int uiIndex)
        {
            (var tile, var location, var actualIndex) = _getBoardDetailsFromIndex(uiIndex);
            if (tile != null)
            {
                RightPressTile(tile, location, actualIndex);
            }
        }



        private static void _processMouseActions(UserInput input)
        {
            if (input.ScrollDifference!=0)
            {
                ScrollWheelTurned(input.ScrollDirection);
            }
            if(input.MouseLeftJustReleased)
            {
                MouseLeftReleased();
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


        public static void LeftClickTile(Tile tile, Vector2Int tileLocation,int actualIndex)
        {
            var distance = _game.FocusEntity.Location.EuclideanDistance(tileLocation);
            if (!Config.EnableRangeLimits || distance <= Config.PlayerRangeLimit)
            {
                switch (SelectedPlayerAction)
                {
                    case PlayerActionSelection.Select:
                        FocusTile(actualIndex);
                        break;
                    case PlayerActionSelection.GiveEnergy:
                        FocusTile(actualIndex);
                        _manualGiveEmergy(tileLocation);
                        break;
                    case PlayerActionSelection.TakeEnergy:
                        FocusTile(actualIndex);
                        _manualTakeEmergy(tileLocation);
                        break;
                    case PlayerActionSelection.CreateEntity:
                        FocusTile(actualIndex);
                        _manualCreateEntity(tileLocation);
                        break;
                    case PlayerActionSelection.DestroyEntity:
                        _manualDestroyEntity(tileLocation);
                        break;
                }
            }
        }


        public static void LeftPressTile(Tile tile, Vector2Int tileLocation, int actualIndex)
        {
            if (tile.Contents != null && (!Config.EnableRangeLimits || _game.FocusEntity.Location.EuclideanDistance(tileLocation) <= Config.PlayerRangeLimit))
            {
                if (tile.Contents.TryLeftPress())
                {
                    AnimationMiscTracker.LaserActive = true;
                    AnimationMiscTracker.LaserDirection = (_game.FocusEntity.Location - CameraOffset, tileLocation - CameraOffset);
                    return;
                }
            }

            AnimationMiscTracker.LaserActive = false;
        }

        public static void MouseLeftReleased()
        {
            AnimationMiscTracker.LaserActive = false;
        }

        public static void RightPressTile(Tile tile, Vector2Int tileLocation, int actualIndex)
        {

        }


        public static void RightClickTile(Tile tile, Vector2Int tileLocation, int actualIndex)
        {
            var distance = _game.FocusEntity.Location.EuclideanDistance(tileLocation);
            if (!Config.EnableRangeLimits || distance <= Config.PlayerRangeLimit)
            {
                _manualDestroyEntity(tileLocation);
            }
        }

        private static void _manualGiveEmergy(Vector2Int tileLocation)
        {
            var dropEnergy = new AdminDropEnergyInstruction();
            dropEnergy.Variables[0] = new LocationVariable(tileLocation);
            dropEnergy.Variables[1] = new IntegerVariable(10);

            _manualInstructionBuffer.Enqueue(dropEnergy);
        }
        private static void _manualTakeEmergy(Vector2Int tileLocation)
        {
            var takeEnergy = new AdminTakeEnergyInstruction();
            takeEnergy.Variables[0] = new LocationVariable(tileLocation);
            takeEnergy.Variables[1] = new IntegerVariable(10);

            _manualInstructionBuffer.Enqueue(takeEnergy);

        }
        private static void _manualCreateEntity(Vector2Int tileLocation)
        {
            var create = new AdminCreateInstruction();
            create.Variables[0] = new LocationVariable(tileLocation);
            create.Variables[1] = new EntityTypeVariable(EntityDatabase.Get(EntityDatabase.Ally2Name));

            _manualInstructionBuffer.Enqueue(create);
        }
        private static void _manualDestroyEntity(Vector2Int tileLocation)
        {
            var destroy = new AdminDestroyInstruction();
            destroy.Variables[0] = new LocationVariable(tileLocation);

            _manualInstructionBuffer.Enqueue(destroy);
        }

        private static (Tile tile, Vector2Int location,int actualIndex) _getBoardDetailsFromIndex(int index)
        {
            (Vector2Int realLocation,int realIndex) = UITileGetRealTile(index);
            var tile = _game.CurrentBoard.TryGetTile(realLocation);

            if (tile != null)
            {
                return (tile, realLocation, realIndex);
            }

            return (null, Vector2Int.MinusOne, -1);
        }
    }
}
