using CubeV2;

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
}
