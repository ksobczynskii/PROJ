namespace PROJ.GameConstansts;

public static class GameConstants
{
    // BOARD
    /// <summary>
    /// Szerokość planszy
    /// </summary>
    public const int Width = 42;
    
    /// <summary>
    /// Wysokość planszy
    /// </summary>
    public const int Height = 22;

    /// <summary>
    /// Górny punkt planszy
    /// </summary>
    public const int BoardTop = 15;

    /// <summary>
    /// Lewy punkt planszy
    /// </summary>
    public const int BoardLeft = 110
        ;

    /// <summary>
    /// Prawy dolny punkt planszy
    /// </summary>
    public static readonly (int x, int y) BoardBottomRight = (BoardLeft+ Width, BoardTop + Height);

    /// <summary>
    /// Znak znad planszy
    /// </summary>
    public const string AboveBoardSign =
        " ██████   ██████                                       ███  ████  ████          \n░░██████ ██████                                       ░░░  ░░███ ░░███          \n ░███░█████░███   ██████   ████████   █████   ██████  ████  ░███  ░███   ██████ \n ░███░░███ ░███  ░░░░░███ ░░███░░███ ███░░   ███░░███░░███  ░███  ░███  ███░░███\n ░███ ░░░  ░███   ███████  ░███ ░░░ ░░█████ ░███████  ░███  ░███  ░███ ░███████ \n ░███      ░███  ███░░███  ░███      ░░░░███░███░░░   ░███  ░███  ░███ ░███░░░  \n █████     █████░░████████ █████     ██████ ░░██████  █████ █████ █████░░██████ \n░░░░░     ░░░░░  ░░░░░░░░ ░░░░░     ░░░░░░   ░░░░░░  ░░░░░ ░░░░░ ░░░░░  ░░░░░░  \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                \n                                                                                ";
    
    /// <summary>
    /// Znak spod planszy
    /// </summary>
    public const string BelowBoardSign =
        " ████  ██████████  ████████     █████   \n░░███ ░███░░░░███ ███░░░░███  ███░░░███ \n ░███ ░░░    ███ ░░░    ░███ ███   ░░███\n ░███       ███     ███████ ░███    ░███\n ░███      ███     ███░░░░  ░███    ░███\n ░███     ███     ███      █░░███   ███ \n █████   ███     ░██████████ ░░░█████░  \n░░░░░   ░░░      ░░░░░░░░░░    ░░░░░░   \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        \n                                        ";
    
    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od góry ma napis znad planszy
    /// </summary>
    public const int SignStartTop = 5;

    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od lewej ściany ma napis znad planszy
    /// </summary>
    public const int SignStartLeft = 92;

    
    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od góry ściany ma napis spod planszy
    /// </summary>
    public const int Sign2StartTop = BoardTop + Height + 2;
    
    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od lewej ściany ma napis spod planszy
    /// </summary>
    public const int Sign2StartLeft = 112;
    
    
    public const char PlayerSymbol = '¶';
    public const char WallSymbol = '█';
    
    //ACTIONBOX

    public const int ActionBoxTop = 55;
    public const int ActionBoxLeft = 92;
    public const int ActionBoxRight = 170;
    public const int ActionBoxBottom = 65;
    public const int ActionBoxWritingPointName = 57;
    public const int ActionBoxWritingPointDesc = ActionBoxWritingPointName + 3;
    public const int ActionBoxWritingPointPickup = ActionBoxWritingPointDesc + 3;
    
    
    public const int VitalsBoxTop = 15;
    public const int VitalsBoxLeft = 40;
    public const int VitalsBoxRight = 70;
    public const int VitalsBoxBottom = 30;
    public const int VitalsBoxWritingPointStartTop = VitalsBoxTop + 3;
    public const int VitalsBoxWritingPointStartLeft = VitalsBoxLeft + 3;
    // public const int VitalsBoxWritingPointDesc = VitalsBoxWritingPointName + 3;
    // public const int VitalsBoxWritingPointPickup = VitalsBoxWritingPointDesc + 3;
    
    public const int WealthBoxTop = 35;
    public const int WealthBoxLeft = 40;
    public const int WealthBoxRight = 70;
    public const int WealthBoxBottom = 45;
    public const int WealthBoxWritingPointStartTop = WealthBoxTop + 3;
    public const int WealthBoxWritingPointStartLeft = WealthBoxLeft + 3;
    
    // BACKPACK
    public const int BackpackCapacity = 10;


    //VITALSBOX
}