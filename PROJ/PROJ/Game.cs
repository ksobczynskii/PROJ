using PROJ.Handlers;
using PROJ.Handlers.Enums;

namespace PROJ;

public class Game
{
    private Board _board;
    private Player _player;
    
    //Boxes
    private ActionBox _actionBox;
    private VitalsBox _vitalsBox;
    private WealthBox _wealthBox;
    private EquipmentBox _eqBox;
    private LeftHandBox _leftHandBox;
    private RightHandBox _rightHandBox;
    private AboveActionErrorSpace _errSpace;
    private PlayerMovesBox _pmBox;

    
    
    public Game()
    {
        _pmBox = new PlayerMovesBox();
        _actionBox = new ActionBox();
        _board = new Board(_actionBox, _pmBox);
        _player = new Player(_board);
        
        _vitalsBox = new VitalsBox(_player);
        _wealthBox = new WealthBox(_player);
        _eqBox = new EquipmentBox(_player);
        _leftHandBox = new LeftHandBox(_player);
        _rightHandBox = new RightHandBox(_player);
        _errSpace = new AboveActionErrorSpace();
        
        
        _player.wBox = _wealthBox;
        _player.eqBox = _eqBox;
        _player.lhBox = _leftHandBox;
        _player.rhBox = _rightHandBox;
        _player.errSpace = _errSpace;

    }

    public void Start()
    {
        Console.Clear();

        Console.CursorVisible = false;
        _board.AddPlayer(_player);
        _board.Generate();
        
        _board.Display();
        // _board.GenerateItems();
        _actionBox.DisplayFrame();
        
        _vitalsBox.DisplayFrame();
        _vitalsBox.DisplayVitals();
        
        _wealthBox.DisplayFrame();
        _wealthBox.DisplayGoods();
        
        _eqBox.DisplayFrame();
        _eqBox.DisplayItems();
        
        _leftHandBox.DisplayFrame();
        _leftHandBox.DisplayHand();
        
        _rightHandBox.DisplayFrame();
        _rightHandBox.DisplayHand();
        
        WaitForMove();
    }
    public void WaitForMove()
    {
        var sh = new SeekHandler(_board);
        var eh = new EscapeHandler(_player);
        var mh = new MoveHandler(_board);
        var puh = new PickUpHandler(_player);
        var bmh = new BackpackModeHandler(_player);
        var dmh = new DisallowedMoveHandler(_errSpace);

        sh.SetNext(eh);
        eh.SetNext(mh);
        mh.SetNext(puh);
        puh.SetNext(bmh);
        bmh.SetNext(dmh);
        while (true)
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            var res = sh.Handle(key);
            if (res == HandleResult.ExitGame)
                return;
        }
    }
    
    public void End()
    {
        Console.WriteLine("Game Ended!");
    }
}