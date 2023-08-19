using CubeV2;
using System;

public class EmptyBoardlessGame : Game
{
    public override BoardTemplateTemplate CreateTemplateTemplate()
    {
        return null;
    }

    public override void RespawnPlayer()
    {
        throw new NotImplementedException();
    }

}