using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class PickUpHandler : Handler
{
    private Player? _player;

    public PickUpHandler(Player p)
    {
        _player = p;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (_player == null)
        {
            if(next != null)
                return next.Handle(key);
            else
            {
                return HandleResult.NotHandled;
            }
        }
        if (key == ConsoleKey.E)
        {
            _player.TryPickUp();
            return HandleResult.Handled;
        }
        if(next != null)
             return next.Handle(key);
        return HandleResult.NotHandled;
    }
}