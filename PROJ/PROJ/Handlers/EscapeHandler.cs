using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class EscapeHandler : Handler
{
    private Player? _player;

    public EscapeHandler(Player p)
    {
        _player = p;
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
            return HandleResult.ExitGame;
        }
        if(next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}