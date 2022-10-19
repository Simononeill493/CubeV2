
using CubeV2;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        RandomUtils.Init();
        DirectionUtils.Init();
        KeyUtils.Init();
        Config.Load();
        InstructionDatabase.Load();

        Testing.Go();

        using var game = new CubeV2.GameWrapper();
        game.Run();
    }
}