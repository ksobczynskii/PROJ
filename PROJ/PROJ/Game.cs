namespace PROJ;

public class Game
{
    private Board _board;
    private Player _player;
    
    //Boxes
    private ActionBox _actionBox;
    private VitalsBox _vitalsBox;
    private WealthBox _wealthBox;
    
    public Game()
    {
        _actionBox = new ActionBox();
        _board = new Board(_actionBox);
        _player = new Player(_board);
        _vitalsBox = new VitalsBox(_player);
        _wealthBox = new WealthBox(_player);
        _player.wBox = _wealthBox;
    }

    public void Start()
    {
        Console.Clear();

        Console.CursorVisible = false;
        _board.AddPlayer(_player);
        
        _board.Display();
        _board.GenerateItems();
        _actionBox.DisplayFrame();
        
        _vitalsBox.DisplayFrame();
        _vitalsBox.DisplayVitals();
        
        _wealthBox.DisplayFrame();
        _wealthBox.DisplayGoods();
        
        WaitForMove();
    }
    public void WaitForMove()
    {
        while (true)
        {
            char key = Console.ReadKey(intercept: true).KeyChar;
            switch (key) // TODO chain of responsibility
            {
                case 'w':
                    _board.MoveUp();
                    break;
                case 'a':
                    _board.MoveLeft();
                    break;
                case 's':
                    _board.MoveDown();
                    break;
                case 'd':
                    _board.MoveRight();
                    break;
                case 'e':
                    _player.TryPickUp();
                    break;
                default:
                    Console.Clear();
                    return;
            }
        }
    }
    
    public void End()
    {
        Console.WriteLine("Game Ended!");
    }
}