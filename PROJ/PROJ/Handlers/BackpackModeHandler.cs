using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class BackpackModeHandler : Handler
{
    private Player? _player;

    public BackpackModeHandler(Player p)
    {
        _player = p;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (_player == null)
        {
            if (next != null)
                return next.Handle(key);
            else
            {
                return HandleResult.NotHandled;
            }
        }
        switch (key)
        {
            case ConsoleKey.B:
            {
                _player.SwitchBackpackMode();
                return HandleResult.Handled;
            }
            case ConsoleKey.UpArrow:
                if (_player.IsInBackpack)
                {
                    _player.TryDecrementBackpackIdx();
                    return HandleResult.Handled;
                }
                if(next != null)
                {
                    return next.Handle(key);
                }
                break;
            case ConsoleKey.DownArrow:
                if (_player.IsInBackpack)
                {
                    _player.TryIncrementBackpackIdx();
                    return HandleResult.Handled;
                }
                if(next != null)
                {
                    return next.Handle(key);
                }
                break;
            case ConsoleKey.R:
                if (_player.IsInBackpack)
                {
                    _player.TrySwap('r');
                    return HandleResult.Handled;
                }
                if(next != null)
                {
                    return next.Handle(key);
                }
                break;
            case ConsoleKey.L:
                if (_player.IsInBackpack)
                {
                    _player.TrySwap('l');
                    return HandleResult.Handled;
                }
                if(next != null)
                {
                    return next.Handle(key);
                }
                break;
            case ConsoleKey.Q:
                if (_player.IsInBackpack)
                {
                    _player.BackpackDrop();
                    return HandleResult.Handled;
                }
                break;
            default:
                if(next != null)
                    return next.Handle(key);
                break;
        }

        return HandleResult.NotHandled;

    }
}