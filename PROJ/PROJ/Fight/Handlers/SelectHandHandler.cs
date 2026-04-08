using PROJ.Handlers;
using PROJ.Handlers.Enums;

namespace PROJ.Fight.Handlers;

public class SelectHandHandler : Handler
{
    private FightMenu _menu;

    public SelectHandHandler(FightMenu menu)
    {
        _menu = menu;
    }

    public override HandleResult Handle(ConsoleKey key)
    {
        if (key == ConsoleKey.L)
        {
            _menu.SetHand('L');
            return HandleResult.Handled;
        }

        if (key == ConsoleKey.R)
        {
            _menu.SetHand('R');
            return HandleResult.Handled;
        }
        if (next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}