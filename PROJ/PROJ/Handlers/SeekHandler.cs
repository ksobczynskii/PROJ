using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class SeekHandler : Handler
{
    private Board? _board;
    public SeekHandler(Board b)
    {
        _board = b;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (_board == null)
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

        if (key == ConsoleKey.LeftArrow)
        {
            _board.TryDecreaseSeek();
            _board.RefreshActionBox();
            return HandleResult.Handled;
        }
        if (key == ConsoleKey.RightArrow)
        {
            _board.TryIncreaseSeek();
            _board.RefreshActionBox();
            return HandleResult.Handled;
        }
        
        _board.ResetSeek();
        if(next != null)
            return next.Handle(key);
        return HandleResult.NotHandled;
    }
}