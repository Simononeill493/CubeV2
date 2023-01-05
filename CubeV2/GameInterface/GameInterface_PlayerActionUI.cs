using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public enum PlayerActionSelection
    {
        Select,
        GiveEnergy,
        TakeEnergy,
        CreateEntity,
        DestroyEntity
    }

    public static class PlayerActionUtils
    {
        public static int _numValues = typeof(PlayerActionSelection).GetEnumValues().Cast<PlayerActionSelection>().Count();

        public static string GetDisplayMessage(PlayerActionSelection selection)
        {
            switch (selection)
            {
                case PlayerActionSelection.Select:
                    return "Select";
                case PlayerActionSelection.GiveEnergy: 
                    return "Give Energy";
                case PlayerActionSelection.TakeEnergy: 
                    return "Take Energy";
                case PlayerActionSelection.CreateEntity: 
                    return "Create";
                case PlayerActionSelection.DestroyEntity: 
                    return "Destroy";
                default: return "???";
            }
        }

        public static bool HasSubSelection(PlayerActionSelection selection)
        {
            switch (selection)
            {
                case PlayerActionSelection.CreateEntity: 
                    return true;
                default:
                    return false;
            }
        }

        public static PlayerActionSelection RotateForward(PlayerActionSelection current)
        {
            return (PlayerActionSelection)(((int)current + 1) % _numValues);
        }

        public static PlayerActionSelection RotateBackwards(PlayerActionSelection current)
        {
            var index = (int)current - 1;
            if(index < 0)
            {
                return (PlayerActionSelection)(_numValues - 1);
            }

            return (PlayerActionSelection)index;
        }

    }

    public partial class GameInterface
    {
        public static PlayerActionSelection SelectedPlayerAction = PlayerActionSelection.Select;
    }
}
