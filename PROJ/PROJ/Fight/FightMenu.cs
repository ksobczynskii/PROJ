using PROJ.Enemies;
using PROJ.Fight.Classes.Visitors;
using PROJ.Fight.Handlers;
using PROJ.Fight.Visitors;
using PROJ.Handlers;
using PROJ.Handlers.Enums;
using PROJ.Logging.Classes;
using PROJ.Tools.Classes;
using PROJ.Tools.Classes.Weapons.Abstract_Types;

namespace PROJ.Fight;

public class FightMenu
{
    private Player _player;
    private Enemy _enemy;
    private FightBox _box;
    private int? _attack;
    private char? _hand;
    private AboveActionErrorSpace _errorSpace;
    private Game _game;
    public FightMenu(Player p, Enemy e, FightBox box, Game g)
    {
        _player = p;
        _enemy = e;
        _box = box;
        _errorSpace = new AboveActionErrorSpace();
        _game = g;
    }

    public bool StartFight()
    {
        var efh = new ExitFightHandler(_box);
        var sah = new SelectAttackHandler(this);
        var shh = new SelectHandHandler(this);
        var ah = new AttackHandler(this);

        efh.SetNext(sah);
        sah.SetNext(shh);
        shh.SetNext(ah);
        _box.FightMode(_player,_enemy);
        while (true)
        {
            if (_enemy.Dead())
                return true;
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            var res = efh.Handle(key);
            if (res == HandleResult.ExitGame)
                return false;
        }
    }

    public void SetAttack(int which)
    {
        _attack = which;
        _box.HighlightAttack(which);
    }

    public void SetHand(char c)
    {
        _hand = c;
        _box.HighlightHand(c, _player);
    }

    public void SimulateAttack()
    {
        
        if(_hand == null)
        {
            _box.HighlightHand('L', _player);
            _hand = 'L';
            
        }

        if (_attack == null)
        {
            _box.HighlightAttack(1);
            _attack = 1;
        }

        AttackVisitor? visitor = null;
        if (_attack == 1) // TODO na enuma
        {
            visitor = new NormalAttackVisitor(_player);
        }
        else if (_attack == 2)
        {
            visitor = new SneakyAttackVisitor(_player);
        }
        else if (_attack == 3)
        {
            visitor = new MagicAttackVisitor(_player);
        }

        if (visitor == null)
        {
            _errorSpace.DisplayErr("UNKNOWN ERROR!");
            return;
        }

        Tool? w = null;

        if (_hand == 'L')
        {
            w = _player.LeftHand;
        }
        else if (_hand == 'R')
        {
            w = _player.RightHand;
        }

        if (w == null)
        {
            _errorSpace.DisplayErr("Cannot Attack with bare hands!");
            return;
        }

        var result = w.Accept(visitor);
        Console.SetCursorPosition(180, 5);
        var logger = Logger.GetInstance;
        // Console.WriteLine($"Got result of attack: damage - {result.DamageToEnemy}, defense - {result.PlayerDefense}");
        // Console.WriteLine($"Attack = {_attack}, Hand = {_hand}");
        _enemy.Hit(result.DamageToEnemy);
        
        logger.Log($"{_player.Name} Hit {_enemy.Name} with {result.DamageToEnemy} damage");
        _box.UpdateEnemyVitals(_enemy);
        Thread.Sleep(50);
        if (_enemy.Dead())
        {
            logger.Log($"{_player.Name} Killed {_enemy.Name}");
            _box.DeadEnemyDisplay(_enemy);
            _enemy.DeleteYourself();
            return;
        }
        _enemy.Attack(_player, result.PlayerDefense);
        logger.Log($"{_enemy.Name} Attacked {_player.Name} with {int.Max(_enemy.Damage - result.PlayerDefense,0)} damage");

        Thread.Sleep(50);
        if (_player.Dead())
        {
            logger.Log($"{_enemy.Name} Killed {_player.Name}");
            _game.EndBad();
        }

        _player.UpdateVitals();
        return;
    }
}