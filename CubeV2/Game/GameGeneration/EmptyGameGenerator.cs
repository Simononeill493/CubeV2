using CubeV2;

internal class EmptyGameGenerator
{
    public static Game CreateEmptyGame()
    {
        var game = new Game();

        game.SetTemplateTemplate(_createEmptyTemplateTemplate());
        game.ResetBoardTemplate();
        game.ResetBoard();

        game.WinCondition = new NoWinCondition();

        return game;
    }

    private static BoardTemplateTemplate _createEmptyTemplateTemplate()
    {
        var templateTemplate = new FullyRandomTemplateTemplate() { Width = Config.GameGridWidth, Height = Config.GameGridHeight };
        return templateTemplate;
    }
}