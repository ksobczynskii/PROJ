using PROJ.Builder.Classes;
using PROJ.Communication.Results;
using PROJ.Enemies;
using PROJ.Fight;
using PROJ.GameConstansts;
using PROJ.Logging.Classes;
using PROJ.Themes;
using PROJ.Tools.Classes;

namespace PROJ;
using System;

public class Board
{

    public Tile[,] Tiles;
    
    private Player _player;
    
    private int _currentlySeeked;
    private readonly List<Player> players = new();
    private readonly Dictionary<Player, int> seekByPlayer = new();
    private readonly Dictionary<Player, FightState> fightsByPlayer = new();


    private List<Enemy> enemies = new();

    private sealed class FightState
    {
        public FightState(FightMenu menu, int enemyRow, int enemyColumn)
        {
            Menu = menu;
            EnemyRow = enemyRow;
            EnemyColumn = enemyColumn;
        }

        public FightMenu Menu { get; }
        public int EnemyRow { get; }
        public int EnemyColumn { get; }
    }
    


    private bool CanMove(int x, int y)
    {
        if (x >= GameConstants.Width-1 || x <= 0 || y >= GameConstants.Height-1 || y <= 0)
            return false;
        return !Tiles[y, x].BlocksMovement && !IsOccupiedByPlayer(x, y, _player);
    }

    private int CurrentSeek
    {
        get
        {
            if (!seekByPlayer.TryGetValue(_player, out int seek))
            {
                seekByPlayer[_player] = 0;
                return 0;
            }

            return seek;
        }
        set
        {
            int seek = Math.Max(0, value);
            seekByPlayer[_player] = seek;
            _currentlySeeked = seek;
        }
    }

    private FightState? CurrentFightState
    {
        get
        {
            return fightsByPlayer.TryGetValue(_player, out FightState? state) ? state : null;
        }
    }

    private Player? GetPlayerAt(int x, int y)
    {
        return players.FirstOrDefault(player =>
            !player.Dead() &&
            player.Position[0] == x &&
            player.Position[1] == y);
    }

    private bool IsOccupiedByPlayer(int x, int y, Player? except = null)
    {
        return players.Any(player =>
            !ReferenceEquals(player, except) &&
            !player.Dead() &&
            player.Position[0] == x &&
            player.Position[1] == y);
    }

    private string GetBlockedByName(int x, int y)
    {
        Player? player = GetPlayerAt(x, y);
        if (player != null && !ReferenceEquals(player, _player))
            return player.Name;

        Tile tile = Tiles[y, x];
        if (tile.Content != null && tile.Content.Count > 0)
            return tile.Content[0].Name;

        return "blocked tile";
    }

    private (int X, int Y) FindSpawnPosition()
    {
        if (Tiles == null)
            return (1, 1);

        for (int y = 1; y < GameConstants.Height - 1; y++)
        {
            for (int x = 1; x < GameConstants.Width - 1; x++)
            {
                if (!Tiles[y, x].BlocksMovement && !IsOccupiedByPlayer(x, y))
                    return (x, y);
            }
        }

        return (1, 1);
    }

    public char GetVisualAt(int x, int y)
    {
        Player? player = GetPlayerAt(x, y);
        if (player != null)
            return player.MapSymbol;

        if (Tiles[y, x].IsEmpty)
            return ' ';
        if (Tiles[y, x].Content != null &&
            Tiles[y, x].Objects > 0) // TODO pusibul nul - teorytycznie miedzy linijkami może się zmienić
            return Tiles[y, x].GetVisual();

        return ' ';
    }

    public TileViewResult CreateTileViewResult(int row, int column)
    {
        return new TileViewResult(row, column, GetVisualAt(column, row), Tiles[row, column].IsEmpty && !IsOccupiedByPlayer(column, row));
    }

    public TileChangeResult CreateTileChangeResult(int row, int column)
    {
        TileViewResult tile = CreateTileViewResult(row, column);
        return new TileChangeResult(tile.Row, tile.Column, tile.Visual, tile.IsEmpty);
    }

    public BoardSnapshotResult CreateSnapshotResult()
    {
        List<TileViewResult> tiles = new();

        for (int row = 0; row < GameConstants.Height; row++)
        {
            for (int column = 0; column < GameConstants.Width; column++)
            {
                tiles.Add(CreateTileViewResult(row, column));
            }
        }

        return new BoardSnapshotResult(tiles);
    }

    public ActionBoxResult CreateActionBoxResult(int row, int column, int seek = 0)
    {
        Tile tile = Tiles[row, column];
        List<BoardObjectViewResult> objects = new();

        if (tile.Content != null)
        {
            foreach (BoardObject obj in tile.Content)
            {
                objects.Add(new BoardObjectViewResult(obj.Name, obj.Description, obj.Pickupable));
            }
        }

        int selected = objects.Count == 0 ? 0 : Math.Clamp(seek, 0, objects.Count - 1);
        return new ActionBoxResult(objects, selected);
    }

    public ActionBoxResult CreatePlayerActionBoxResult(int seek = -1)
    {
        int column = _player.Position[0];
        int row = _player.Position[1];
        return CreateActionBoxResult(row, column, seek >= 0 ? seek : CurrentSeek);
    }

    public EnemyViewResult? CreateNearbyEnemyResult()
    {
        Enemy? enemy = GetNearestEnemy();
        return enemy != null ? CreateEnemyViewResult(enemy) : null;
    }

    public PlayerFightViewResult CreatePlayerFightResult()
    {
        return new PlayerFightViewResult(
            _player.LeftHand != null ? _player.LeftHand.Visual : GameConstants.EmptyHandSymbol,
            _player.RightHand != null ? _player.RightHand.Visual : GameConstants.EmptyHandSymbol);
    }

    public bool IsFightActive => CurrentFightState != null;

    private static EnemyViewResult CreateEnemyViewResult(Enemy enemy)
    {
        return new EnemyViewResult(enemy.Name, enemy.Description, enemy.Fightable, enemy.Visual, enemy.Health, enemy.Armor);
    }

    public TileBlinkResult? CreateTileBlinkResult(int row, int column, ConsoleColor color)
    {
        if (row < 0 || row >= Tiles.GetLength(0) || column < 0 || column >= Tiles.GetLength(1))
            return null;

        return new TileBlinkResult(CreateTileViewResult(row, column), color);
    }

    private List<MoveResult> PrepareMoveResults(List<MoveResult> results)
    {
        foreach (MoveResult result in results)
        {
            if (!result._success)
                continue;

            result.SetRenderedTiles(
                CreateTileViewResult(result.from_y, result.from_x),
                CreateTileViewResult(result.to_y, result.to_x));
        }

        if (results.Count > 0)
        {
            results[0].SetViewContext(
                results[0]._success ? CreatePlayerActionBoxResult() : null,
                CreateNearbyEnemyResult());
        }

        return results;
    }

    public void InsertEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }
    public void Init(Tile[,] tiles)
    {
        Tiles = tiles;
    }
    

    public TileChangeResult RemoveFromMap(int x, int y)
    {
        Tiles[x,y].Remove(CurrentSeek);
        return CreateTileChangeResult(x,y);
    }
    public Board() // problem - potrzebuje playera do wywolania generate - Pamietac ze miedzy konstruktorem a generate musi byc player assignment
    {
        _currentlySeeked = 0;
    }

    public Player GetPlayer => _player;

    // private void CentralHallWithLootgenerate(DungeonBuilder builder)
    // {
    //     builder.CreateEmptyDungeon();
    //     builder.AddCentralHall(12,13);
    //     builder.AddWeapons(30);
    //     builder.AddItems(20);
    // }
    //
    // private void ManySmallRoomsWithLootGenerate(DungeonBuilder builder)
    // {
    //     builder.CreateEmptyDungeon();
    //     // builder.AddCentralHall(12,13);
    //     builder.AddRooms(30);
    //     builder.AddWeapons(30);
    //     builder.AddItems(20);
    // }

    // private void OnlyMazeGenerate(DungeonBuilder builder)
    // {
    //     builder.CreateFilledDungeon();
    //     // builder.AddCorridors(10);
    // }
    //
    // private void MixGenerate(DungeonBuilder builder)
    // {
    //     builder.CreateEmptyDungeon();
    //     // builder.CreateFilledDungeon();
    //     builder.AddCentralHall(10,7);
    //     builder.AddRooms(10);
    //     builder.AddCorridors(10);
    //     builder.AddWeapons(20);
    //     builder.AddItems(10);
    //     builder.AddEnemies(5);
    // }
    //
    
    



    public BoardObject? GetCurrentlySeeked()
    {
        var tiles = Tiles[_player.Position[1], _player.Position[0]].Content;
        int currentSeek = CurrentSeek;
        if (tiles == null || Tiles[_player.Position[1], _player.Position[0]].Objects <= currentSeek)
            return null;
        return tiles[currentSeek];
    }

    public void ResetSeek()
    {
        CurrentSeek = 0;
    }

    public int TryIncreaseSeek()
    {
        int currentSeek = CurrentSeek;
        if (currentSeek < Tiles[_player.Position[1], _player.Position[0]].Objects - 1)
        {
            CurrentSeek = currentSeek + 1;
        }
        return CurrentSeek;
    }
    
    public int TryDecreaseSeek()
    {
        int currentSeek = CurrentSeek;
        if (currentSeek > 0)
            CurrentSeek = currentSeek - 1;
        return CurrentSeek;
    }

    public void AddPlayer(Player player, string? playerName = null, int playerId = 0)
    {
        if (!players.Contains(player))
            players.Add(player);

        _player = player;
        player.AssignBoard(this);
        player.NetworkId = playerId;
        player.Name = playerName != null ? playerName : playerId > 0 ? $"Player {playerId}" : "Player 1";

        var spawn = FindSpawnPosition();
        player.Position[0] = spawn.X;
        player.Position[1] = spawn.Y;
        CurrentSeek = 0;
    }

    public void SetActivePlayer(Player player)
    {
        if (!players.Contains(player))
            AddPlayer(player);

        _player = player;
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
        seekByPlayer.Remove(player);
        fightsByPlayer.Remove(player);

        if (ReferenceEquals(_player, player) && players.Count > 0)
            _player = players[0];
    }
    
    public List<MoveResult> MoveRight()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x + 1;
        int newY = y;
        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {GetBlockedByName(newX, newY)} at {newX}, {newY}");
            var list = EnemiesTurn();
            return PrepareMoveResults(new List<MoveResult> { new(x, y, newX, newY, false) }.Concat(list).ToList());
        }
            
        _player.Position[0] = newX;
        _player.Position[1] = newY;
        var mr = new MoveResult(x, y, newX, newY, true);
        var listAfterMove = EnemiesTurn();
        return PrepareMoveResults(new List<MoveResult> { mr }.Concat(listAfterMove).ToList());
    }
    public List<MoveResult> MoveLeft()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x - 1;
        int newY = y;
        
        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {GetBlockedByName(newX, newY)} at {newX}, {newY}");
            var list = EnemiesTurn();
            return PrepareMoveResults(new List<MoveResult> { new(x, y, newX, newY, false) }.Concat(list).ToList());
        }
            
        _player.Position[0] = newX;
        _player.Position[1] = newY;
        var mr = new MoveResult(x, y, newX, newY, true);
        var listAfterMove = EnemiesTurn();
        return PrepareMoveResults(new List<MoveResult> { mr }.Concat(listAfterMove).ToList());
    }
    public List<MoveResult> MoveDown()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y + 1;

        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {GetBlockedByName(newX, newY)} at {newX}, {newY}");
            var list = EnemiesTurn();
            return PrepareMoveResults(new List<MoveResult> { new(x, y, newX, newY, false) }.Concat(list).ToList());
        }
        _player.Position[0] = newX;
        _player.Position[1] = newY;
        var mr = new MoveResult(x, y, newX, newY, true);
        var listAfterMove = EnemiesTurn();
        return PrepareMoveResults(new List<MoveResult> { mr }.Concat(listAfterMove).ToList());
    }
    public List<MoveResult> MoveUp()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y - 1;

        var logger = Logger.GetInstance;
        
        
        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {GetBlockedByName(newX, newY)} at {newX}, {newY}");
            var l = EnemiesTurn();
            return PrepareMoveResults(new List<MoveResult>{new (x,y,newX,newY,false)}.Concat(l).ToList());
        }

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        
        var mr = new MoveResult(x,y,newX,newY, true);
        // _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        var list = EnemiesTurn();
        // _fightBox.AfterMoveAssesment(GetNearestEnemy());
        return PrepareMoveResults(new List<MoveResult>{mr}.Concat(list).ToList());
    }
    
 
    // public void RefreshActionBox(int x, int y)
    // {
    //     _actionBox.AfterMoveAsessment(Tiles[x,y].Content,Tiles[x,y].Objects); 
    // }
    //
    // public void RefreshActionBox()
    // {
    //     int x = _player.Position[1];
    //     int y = _player.Position[0];
    //     _actionBox.AfterMoveAsessment(Tiles[x,y].Content,Tiles[x,y].Objects, _currentlySeeked); 
    // }

    public void DropItem(Tool tool)
    {
        int x = _player.Position[1];
        int y = _player.Position[0];
        tool.X = x;
        tool.Y = y;
        tool.ObjBoard = this;
        Tiles[x,y].AddObj(tool);
    }

    private bool IsEnemy(int x, int y)
    {
        if (Tiles[y, x].Objects > 0 && Tiles[y, x].Content[0].Fightable) // TODO WIEM WIEM WIEM KURCZe
            return true;
        return false;
    }
    

    public Enemy? GetNearestEnemy()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        if (y> 1 && IsEnemy(x, y - 1))
            return (Enemy)Tiles[y-1,x].Content[0];
        if (y < GameConstants.Height - 2 && IsEnemy(x, y+1))
            return (Enemy)Tiles[y+1,x].Content[0];
        if (x > 0 && IsEnemy(x-1, y))
            return (Enemy)Tiles[y,x-1].Content[0];
        if (x < GameConstants.Width - 2 && IsEnemy(x + 1, y))
            return (Enemy)Tiles[y, x + 1].Content[0];
        return null;
    }

    public FightStartResult FightNearestEnemy()
    {
        if (_player.Position[1] > 1 && IsEnemy(_player.Position[0], _player.Position[1] - 1))
        {
            return BeginFight(_player.Position[1] - 1, _player.Position[0]);
        }

        if (_player.Position[1] < GameConstants.Height - 2 && IsEnemy(_player.Position[0], _player.Position[1] + 1))
        {
            return BeginFight(_player.Position[1] + 1, _player.Position[0]);
        }

        if (_player.Position[0] > 0 && IsEnemy(_player.Position[0] - 1, _player.Position[1]))
        {
            return BeginFight(_player.Position[1], _player.Position[0] - 1);
        }

        if (_player.Position[0] < GameConstants.Width - 2 && IsEnemy(_player.Position[0] + 1, _player.Position[1]))
        {
            return BeginFight(_player.Position[1], _player.Position[0] + 1);
        }

        return new FightStartResult(false, errorMessage: "No Enemies Nearby to fight");
    }

    public FightLoopEndResult RunCurrentFight()
    {
        FightState? fightState = CurrentFightState;
        if (fightState == null)
        {
            return new FightLoopEndResult(false, false, false);
        }

        FightLoopEndResult result = fightState.Menu.StartFight();

        if (result.EnemyKilled)
        {
            Tiles[fightState.EnemyRow, fightState.EnemyColumn].Reset();
            result = new FightLoopEndResult(true, result.ExitedByPlayer, result.PlayerDied,
                CreateTileChangeResult(fightState.EnemyRow, fightState.EnemyColumn));
        }

        fightsByPlayer.Remove(_player);
        return result;
    }

    private FightStartResult BeginFight(int enemyRow, int enemyColumn)
    {
        bool enemyAlreadyEngaged = fightsByPlayer.Values.Any(fight =>
            fight.EnemyRow == enemyRow && fight.EnemyColumn == enemyColumn);
        if (enemyAlreadyEngaged)
            return new FightStartResult(false, errorMessage: "Enemy already fighting another player");

        Enemy? enemy = Tiles[enemyRow, enemyColumn].TryGetEnemy();
        if (enemy == null)
        {
            return new FightStartResult(false, errorMessage: "No Enemies Nearby to fight");
        }

        fightsByPlayer[_player] = new FightState(new FightMenu(_player, enemy), enemyRow, enemyColumn);
        return new FightStartResult(true, CreateEnemyViewResult(enemy), CreatePlayerFightResult());
    }

    public EnemyViewResult? CreateCurrentFightEnemyResult()
    {
        FightState? fightState = CurrentFightState;
        if (fightState == null)
            return null;

        Enemy? enemy = Tiles[fightState.EnemyRow, fightState.EnemyColumn].TryGetEnemy();
        return enemy != null ? CreateEnemyViewResult(enemy) : null;
    }

    public FightAttackSelectionResult? SelectCurrentFightAttack(int attack)
    {
        return CurrentFightState?.Menu.SetAttack(attack);
    }

    public FightHandSelectionResult? SelectCurrentFightHand(char hand)
    {
        return CurrentFightState?.Menu.SetHand(hand);
    }

    public int? CurrentFightAttack => CurrentFightState?.Menu.CurrentAttack;

    public char? CurrentFightHand => CurrentFightState?.Menu.CurrentHand;

    public FightExitResult? ExitCurrentFight()
    {
        FightState? fightState = CurrentFightState;
        if (fightState == null)
            return null;

        FightExitResult result = fightState.Menu.ExitFight();
        fightsByPlayer.Remove(_player);
        return result;
    }

    public FightTurnResult? SimulateCurrentFightAttack()
    {
        FightState? fightState = CurrentFightState;
        if (fightState == null)
            return null;

        FightTurnResult result = fightState.Menu.SimulateAttack();

        if (result.EnemyDead)
        {
            Tiles[fightState.EnemyRow, fightState.EnemyColumn].Reset();
            fightsByPlayer.Remove(_player);
        }

        if (result.PlayerDead || result.ExitFightMode)
            fightsByPlayer.Remove(_player);

        return result;
    }
    
    
    

    private MoveResult? EnemyMove(Enemy e)
    {
        Random rnd = new Random();
        int x = rnd.Next(0, 100);
        if (x < 33)
            return null;
        x = rnd.Next(0, 4);

        MoveResult? move = null;
        switch (x)
        {
            case 0:
            {
                if (e.X == 0 || !Tiles[e.X - 1, e.Y].IsEmpty || IsOccupiedByPlayer(e.Y, e.X - 1))
                    return null;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.X--;
                Tiles[e.X, e.Y].AddObj(e);
                move =  new MoveResult(oldColumn, oldRow, e.Y, e.X, true);
                break;
            }
            case 1:
            {
                if (e.Y == GameConstants.Width - 1 || !Tiles[e.X, e.Y + 1].IsEmpty || IsOccupiedByPlayer(e.Y + 1, e.X))
                    return null;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.Y++;
                Tiles[e.X, e.Y].AddObj(e);
                move =  new MoveResult(oldColumn, oldRow, e.Y, e.X, true);
                break;
            }
            case 2:
            {
                if (e.X == GameConstants.Height - 1 || !Tiles[e.X + 1, e.Y].IsEmpty || IsOccupiedByPlayer(e.Y, e.X + 1))
                    return null;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.X++;
                Tiles[e.X, e.Y].AddObj(e);
                move =  new MoveResult(oldColumn, oldRow, e.Y, e.X, true);
                break;
            }
            case 3:
            {
                if (e.Y == 0 || !Tiles[e.X, e.Y - 1].IsEmpty || IsOccupiedByPlayer(e.Y - 1, e.X))
                    return null;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.Y--;
                Tiles[e.X, e.Y].AddObj(e);
                move =  new MoveResult(oldColumn, oldRow, e.Y, e.X, true);
                break;
            }
        }
        return move;


    }
    private List<MoveResult> EnemiesTurn()
    {
        var list = new List<MoveResult>();
        foreach (var enemy in enemies)
        {
            var res = EnemyMove(enemy);
            if(res != null)
                list.Add(res);
        }
        return list;
    }

    public void DeleteEnemy(Enemy e)
    {
        enemies.Remove(e);
    }
    
    
}
