using PROJ.Handlers;
using PROJ.Handlers.Enums;

namespace PROJ.Fight.Handlers;

public class ExitFightHandler : Handler
{
    private FightBox _box;
    public ExitFightHandler(FightBox box)
    {
        _box = box;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (key == ConsoleKey.Escape)
        {
            _box.Clear();
            return HandleResult.ExitGame;
        }
        if(next!=null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}