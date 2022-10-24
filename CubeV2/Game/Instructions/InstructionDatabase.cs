using System.Collections.Generic;

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
            //_masterList.Add(new MoveRandomlyInstruction());
            _masterList.Add(new HitInstruction());
            _masterList.Add(new PingInstruction());
            _masterList.Add(new ScanInstruction());
            _masterList.Add(new TurnInstruction());
            _masterList.Add(new RotateInstruction());
            _masterList.Add(new StoreDataInstruction());
            _masterList.Add(new IfInstruction());
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
    }
}
