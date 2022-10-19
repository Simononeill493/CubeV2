﻿using System.Collections.Generic;

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
            _masterList.Add(new MoveRandomlyInstruction());

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
