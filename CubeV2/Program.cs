
using CubeV2;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        Config.Load();
        RandomUtils.Init();
        DirectionUtils.Init();
        KeyUtils.Init();
        InstructionDatabase.Load();
        EntityDatabase.Load();

        Testing.Go();

        using var game = new CubeV2.GameWrapper();
        game.Run();
    }
}