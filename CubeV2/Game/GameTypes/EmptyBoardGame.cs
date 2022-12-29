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
        var templateTemplate = new FullyRandomTemplateTemplate() { Width = Config.GameGridDefaultWidth, Height = Config.GameGridDefaultHeight };
        return templateTemplate;
    }
}
