using MonoGame.Framework.Utilities.Deflate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class HarvestableEntity : Entity
    {
        public HarvestableEntity(string templateID, string ID, string sprite,int harvestCount) : base(templateID, ID, sprite)
        {
            _maxHarvestCount = harvestCount;
            _currentHarvestCount = harvestCount;
        }

        public override bool TryLeftPress()
        {
            ShowHarvestMeter = true;

            _currentHarvestCount--;
            if(_currentHarvestCount < 1)
            {
                Harvest();
            }
            
            return true;
        }

        public void Harvest()
        {
            GameInterface._game.CurrentBoard.TryRemoveFromBoard(this, force: true);
        }
    }
}
