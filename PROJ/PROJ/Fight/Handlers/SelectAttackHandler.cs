using PROJ.Handlers;
using PROJ.Handlers.Enums;

namespace PROJ.Fight.Handlers;

public class SelectAttackHandler : Handler
{
    private FightMenu _menu;
    public SelectAttackHandler(FightMenu menu)
    {
        _menu = menu;
    }

    public override HandleResult Handle(ConsoleKey key)
    {
        if (key == ConsoleKey.D1)
        {
            _menu.SetAttack(1);
            return HandleResult.Handled;
        }
        if (key == ConsoleKey.D2)
        {
            _menu.SetAttack(2);
            return HandleResult.Handled;
        }
        if (key == ConsoleKey.D3)
        {
            _menu.SetAttack(3);
            return HandleResult.Handled;
        }

        if (next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}