using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public static class InstructionDatabase
    {
        private static List<Instruction> _masterList;

        public static void Load()
        {
            _loadMasterList();
        }

        private static void _loadMasterList()
        {
            _masterList = new List<Instruction>();

            _masterList.Add(new MoveInstruction());
            _masterList.Add(new HitInstruction());
            _masterList.Add(new PingInstruction());
            _masterList.Add(new ScanInstruction());
            _masterList.Add(new TurnInstruction());
            _masterList.Add(new RotateInstruction());
            _masterList.Add(new StoreDataInstruction());
            _masterList.Add(new IfInstruction());
            _masterList.Add(new SendEnergyInstruction());
        }

        public static List<Instruction> GetAll()
        {
            var output = new List<Instruction>();
            foreach (var inst in _masterList)
            {
                output.Add(inst.GenerateNew());
            }
            return output;
        }

        public static Instruction GenerateRandom() => GenerateRandom(1).First();

        public static List<Instruction> GenerateRandom(int count)
        {
            var output = new List<Instruction>();

            for(int i=0;i<count;i++)
            {
                var rand = RandomUtils.GetRandom(_masterList).GenerateNewFilled();
                output.Add(rand);
            }

            return output;
        }
    }
}
