using PROJ.Handlers;
using PROJ.Handlers.Enums;

namespace PROJ.Fight.Handlers;

public class AttackHandler : Handler
{
    
    private FightMenu _menu;
    public AttackHandler(FightMenu menu)
    {
        _menu = menu;
    }
    
    public override HandleResult Handle(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
        {
            _menu.SimulateAttack();
            return HandleResult.Handled;
        }
        if (next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}