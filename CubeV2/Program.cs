
using CubeV2;
using SAME;
using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        //new years 

        Config.Load();
        RandomUtils.Init();
        DirectionUtils.Init();
        KeyUtils.Init();
        InstructionDatabase.Load();
        EntityDatabase.Load();
        EntityAIDatabase.Load();
        VariableUtils.Init();

        Testing.Go();

        using var game = new CubeV2.CubeV2GameWrapper();
        game.Run();
    }
}