using PROJ.Configuration;
using PROJ.Handlers;
using PROJ.Handlers.Enums;
using PROJ.Logging.Classes;

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
    private FightBox _fightBox;

    
    
    public Game()
    {
        _actionBox = new ActionBox();
        _fightBox = new FightBox();
        _pmBox = new PlayerMovesBox();
        _board = new Board(_actionBox, _pmBox, _fightBox, this);
        _player = _board.GetPlayer;

        _vitalsBox = _player.GetVitalsBox;
        _wealthBox = new WealthBox(_player);
        _eqBox = new EquipmentBox(_player);
        _leftHandBox = new LeftHandBox(_player);
        _rightHandBox = new RightHandBox(_player);
        _errSpace = new AboveActionErrorSpace();
        
        
        _player.WBox = _wealthBox;
        _player.EqBox = _eqBox;
        _player.LhBox = _leftHandBox;
        _player.RhBox = _rightHandBox;
        _player.ErrSpace = _errSpace;

    }

    public void Start()
    {
        var config = Configurator.Instance.Configure();
        Console.Clear();
        var logger = Logger.GetInstance;

        var themeSettings = Configurator.Instance.ConfigureTheme();
        
        

        Console.CursorVisible = false;
        _board.AddPlayer(_player, config.PlayerName);
        _board.Generate(themeSettings);
        
        _board.Display();
        // _board.GenerateItems();
        _actionBox.DisplayFrame();
        _fightBox.DisplayFrame();
        
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
        var eh = new EscapeHandler(_player,this);
        var mh = new MoveHandler(_board);
        var puh = new PickUpHandler(_player);
        var bmh = new BackpackModeHandler(_player);
        var fh = new FightHandler(_board, _errSpace);
        var lm = new LoggerMode(_board);
        var dmh = new DisallowedMoveHandler(_errSpace);
        
        fh.SetNext(sh);
        sh.SetNext(eh);
        eh.SetNext(mh);
        mh.SetNext(puh);
        puh.SetNext(lm);
        lm.SetNext(bmh);
        bmh.SetNext(dmh);
        
        while (true)
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            var res = fh.Handle(key);
            if (res == HandleResult.ExitGame)
                return;
        }
    }
    
    public void EndGood()
    {
        Console.WriteLine("Game Ended!");
        Environment.Exit(0);
    }

    public void EndBad()
    {
        var endScreen = new EndScreen();
        endScreen.EndGame();
        Thread.Sleep(3000);
        Environment.Exit(0);
    }
}
