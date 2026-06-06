namespace PROJ.Communication.Results;

public class MoveResult
{
    public int from_x;
    public int from_y;
    public int to_x;
    public int to_y;
    public bool _success;
    private TileViewResult? _fromTile;
    private TileViewResult? _toTile;
    private ActionBoxResult? _actionBoxResult;
    private EnemyViewResult? _nearbyEnemy;

    public MoveResult(int from_x, int from_y, int to_x, int to_y, bool success)
    {
        this.from_x = from_x;
        this.from_y = from_y;
        this.to_x = to_x;
        this.to_y = to_y;
        _success = success;
    }

    public TileViewResult? FromTile => _fromTile;
    public TileViewResult? ToTile => _toTile;
    public ActionBoxResult? ActionBoxResult => _actionBoxResult;
    public EnemyViewResult? NearbyEnemy => _nearbyEnemy;

    public void SetRenderedTiles(TileViewResult fromTile, TileViewResult toTile)
    {
        _fromTile = fromTile;
        _toTile = toTile;
    }

    public void SetViewContext(ActionBoxResult? actionBoxResult, EnemyViewResult? nearbyEnemy)
    {
        _actionBoxResult = actionBoxResult;
        _nearbyEnemy = nearbyEnemy;
    }
}
