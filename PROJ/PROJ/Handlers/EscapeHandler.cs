using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class EscapeHandler : Handler
{
    private Player? _player;
    private Game _game;

    public EscapeHandler(Player p, Game g)
    {
        _player = p;
        _game = g;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (_player == null)
        {
            if (next != null)
            {
                return next.Handle(key);
            }
            else
            {
                return HandleResult.NotHandled;
            }
        }
        if (key == ConsoleKey.Escape)
        {
            Console.Clear();
            _game.EndGood();
        }
        if(next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}