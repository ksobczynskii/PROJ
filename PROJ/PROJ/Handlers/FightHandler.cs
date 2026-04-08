using PROJ.Handlers.Enums;

namespace PROJ.Handlers;

public class FightHandler: Handler
{
    private Board _board;
    private AboveActionErrorSpace _errSpace;

    public FightHandler(Board board, AboveActionErrorSpace es)
    {
        _board = board;
        _errSpace = es;
    }
    public override HandleResult Handle(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
        {
            if (_board.HasEnemiesNearby())
            {
                _board.FightNearestEnemy();
            }
            else
            {
                _errSpace.DisplayErr("No Enemies Nearby to fight");
            }

            return HandleResult.Handled;
        }
        else
        {
            if(next != null)
                next.Handle(key);
            return HandleResult.NotHandled;
        }
    }
}