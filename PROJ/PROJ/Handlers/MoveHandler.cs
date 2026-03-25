using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class MoveHandler : Handler
{
    private Board? _board;

    public MoveHandler(Board b)
    {
        _board = b;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (_board == null)
        {
            if(next != null)
                return next.Handle(key);
            else
            {
                return HandleResult.NotHandled;
            }
        }
            
        switch (key)
        {
            case ConsoleKey.W:
                _board.MoveUp();
                return HandleResult.Handled;
            case ConsoleKey.A:
                _board.MoveLeft();
                return HandleResult.Handled;
            case ConsoleKey.S:
                _board.MoveDown();
                return HandleResult.Handled;
            case ConsoleKey.D:
                _board.MoveRight();
                return HandleResult.Handled;
            default:
                if(next != null)
                    return next.Handle(key);
                return HandleResult.NotHandled;
        }
    }
}