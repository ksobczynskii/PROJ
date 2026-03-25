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
    public const int BoardLeft = 65
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
    public const int SignStartLeft = 47;

    
    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od góry ściany ma napis spod planszy
    /// </summary>
    public const int Sign2StartTop = BoardTop + Height + 2;

    /// <summary>
    /// Ta zmienna mówi o tym ile pikseli od lewej ściany ma napis spod planszy
    /// </summary>
    public const int Sign2StartLeft = 67;
    
    public const char PlayerSymbol = '¶';
    public const char WallSymbol = '█';
    
    //ACTIONBOX

    public const int ActionBoxTop = 50;
    public const int ActionBoxLeft = 47;
    public const int ActionBoxRight = 125;
    public const int ActionBoxBottom = 58;
    public const int ActionBoxWritingPointName = 51;
    public const int ActionBoxWritingPointDesc = ActionBoxWritingPointName + 3;
    public const int ActionBoxWritingPointPickup = ActionBoxWritingPointDesc + 3;
    
    //VITALSBOX
    public const int VitalsBoxTop = 15;
    public const int VitalsBoxLeft = 20;
    public const int VitalsBoxRight = 40;
    public const int VitalsBoxBottom = 30;
    public const int VitalsBoxWritingPointStartTop = VitalsBoxTop + 3;
    public const int VitalsBoxWritingPointStartLeft = VitalsBoxLeft + 3;
    // public const int VitalsBoxWritingPointDesc = VitalsBoxWritingPointName + 3;
    // public const int VitalsBoxWritingPointPickup = VitalsBoxWritingPointDesc + 3;
    
    // WEALTHBOX
    public const int WealthBoxTop = 35;
    public const int WealthBoxLeft = 20;
    public const int WealthBoxRight = 40;
    public const int WealthBoxBottom = 45;
    public const int WealthBoxWritingPointStartTop = WealthBoxTop + 3;
    public const int WealthBoxWritingPointStartLeft = WealthBoxLeft + 3;
    
    // EQUIPMENTBOX
    public const int EqBoxTop = 15;
    public const int EqBoxLeft = 125;
    public const int EqBoxRight = 155
        ;
    public const int EqBoxBottom = 30;
    public const int EqBoxWritingPointStartTop = EqBoxTop + 3;
    public const int EqBoxWritingPointStartLeft = EqBoxLeft + 3;

    public const int EqPointer = EqBoxRight - 2;
    
    // LeftHandBOX
    
    public const int LeftHandBoxTop = 35;
    public const int LeftHandBoxLeft = 125;
    public const int LeftHandBoxRight = 155
        ;
    
    public const int LeftHandBoxBottom = 45;
    public const int LeftHandoxWritingPointStartTopName = LeftHandBoxTop + 3;
    public const int LeftHandBoxWritingPointStartLeftName = LeftHandBoxLeft + 3;
    
    public const int LeftHandoxWritingPointStartTopDesc = LeftHandoxWritingPointStartTopName + 3;
    
    //RightHandBOX
    public const int RightHandBoxTop = 35;
    public const int RightHandBoxLeft = 175;
    
    public const int RightHandBoxRight = 205;
    
    public const int RightHandBoxBottom = 45;
    public const int RightHandoxWritingPointStartTopName = RightHandBoxTop + 3;
    public const int RightHandBoxWritingPointStartLeftName = RightHandBoxLeft + 3;
    
    public const int RightHandoxWritingPointStartTopDesc = RightHandoxWritingPointStartTopName + 3;    
    // BACKPACK
    public const int BackpackCapacity = 5;
    
    //AboveActionErrorSpace

    public const int ErrorSpaceTop = ActionBoxTop - 2;
    
    
    //PlayerMovesBox
    public const int PlayerMovesBoxLeft = 175;
    
    public const int PlayerMovesBoxRight = 205;
    public const int PlayerMovesBoxTop = 15;
    public const int PlayerMovesBoxBottom = 30;
    public const int PlayerMovesBoxWritingPointStartLeft = PlayerMovesBoxLeft + 3;
    public const int PlayerMovesBoxWritingPointStartTop = PlayerMovesBoxTop + 3;
    
    
    
    //DungeonBuilding

    public const int RoomInsertAttempts = 20;


    //VITALSBOX
}