using PROJ.Handlers.Enums;
using PROJ.Handlers.Interfaces;

namespace PROJ.Handlers;

public class DisallowedMoveHandler : Handler
{
    private AboveActionErrorSpace _errSpace;

    public DisallowedMoveHandler(AboveActionErrorSpace errorSpace)
    {
        _errSpace = errorSpace;
    }
    
    public override HandleResult Handle(ConsoleKey key)
    {
        _errSpace.DisplayErr("Command Not Recognized");
        return HandleResult.Handled;
    }
    
}