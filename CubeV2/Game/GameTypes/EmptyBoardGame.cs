using CubeV2;
using System;

internal class EmptyBoardGame : Game
{
    public EmptyBoardGame() : base()
    {
        SetTemplateTemplate(CreateTemplateTemplate());
        ResetBoardTemplate();
        ResetBoard();

        WinCondition = new NoWinCondition();
    }

    public override BoardTemplateTemplate CreateTemplateTemplate()
    {
        var templateTemplate = new FullyRandomTemplateTemplate() { Width = 16, Height = 16 };
        return templateTemplate;
    }

    public override void RespawnPlayer()
    {
        throw new NotImplementedException();
    }
    public override void OnPlayerDeath()
    {
        throw new NotImplementedException();
    }


}
