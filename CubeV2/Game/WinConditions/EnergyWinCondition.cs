using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class EnergyWinCondition : BoardWinCondition
    {
        private Entity _entityToCheck;
        private int _requiredEnergy;

        public EnergyWinCondition(Entity toCheck, int requiredEnergy)
        {
            _entityToCheck = toCheck;
            _requiredEnergy = requiredEnergy;
        }

        public override bool Check(Board board)
        {
            return _entityToCheck.CurrentEnergy >= _requiredEnergy;
        }
    }
}
