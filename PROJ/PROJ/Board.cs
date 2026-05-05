using PROJ.Builder.Classes;
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

    private ActionBox _actionBox;

    private FightBox _fightBox;
    
    private int _currentlySeeked;
    private PlayerMovesBox _pmBox;

    public bool FightMode;
    private FightMenu _menu;

    private Game _game;

    private List<Enemy> enemies;
    


    private bool CanMove(int x, int y)
    {
        if (x >= GameConstants.Width-1 || x <= 0 || y >= GameConstants.Height-1 || y <= 0)
            return false;
        return !Tiles[y, x].BlocksMovement;
    }
    private char GetVisualAt(int x, int y)
    {
        if (_player.Position[0] == x && _player.Position[1] == y)
            return GameConstants.PlayerSymbol;

        if (Tiles[y, x].IsEmpty)
            return ' ';
        if (Tiles[y, x].Content != null &&
            Tiles[y, x].Objects > 0) // TODO pusibul nul - teorytycznie miedzy linijkami może się zmienić
            return Tiles[y, x].GetVisual();

        return ' ';
    }

    private void DrawAt(int x, int y, char symbol, ConsoleColor? color = null)
    {
        ConsoleRender.WriteAt(GameConstants.BoardLeft + x, GameConstants.BoardTop + y, symbol, color);
    }

    private void DrawTile(int row, int column, ConsoleColor? color = null)
    {
        DrawAt(column, row, GetVisualAt(column, row), color);
    }

    public void RemoveFromMap(int x, int y)
    {
        Tiles[x,y].Remove(_currentlySeeked);
        DrawAt(y,x,GetVisualAt(y,x));
        
    }
    public Board(ActionBox actionBox, PlayerMovesBox pmBox, FightBox fightBox, Game g) // problem - potrzebuje playera do wywolania generate - Pamietac ze miedzy konstruktorem a generate musi byc player assignment
    {
        _currentlySeeked = 0;
        _actionBox = actionBox;
        _pmBox = pmBox;
        _fightBox = fightBox;
        _player = new Player(this);
        FightMode = false;
        _game = g;
    }

    public Player GetPlayer => _player;

    private void CentralHallWithLootgenerate(DungeonBuilder builder)
    {
        builder.CreateEmptyDungeon();
        builder.AddCentralHall(12,13);
        builder.AddWeapons(30);
        builder.AddItems(20);
    }
    
    private void ManySmallRoomsWithLootGenerate(DungeonBuilder builder)
    {
        builder.CreateEmptyDungeon();
        // builder.AddCentralHall(12,13);
        builder.AddRooms(30);
        builder.AddWeapons(30);
        builder.AddItems(20);
    }

    private void OnlyMazeGenerate(DungeonBuilder builder)
    {
        builder.CreateFilledDungeon();
        // builder.AddCorridors(10);
    }

    private void MixGenerate(DungeonBuilder builder)
    {
        builder.CreateEmptyDungeon();
        // builder.CreateFilledDungeon();
        builder.AddCentralHall(10,7);
        builder.AddRooms(10);
        builder.AddCorridors(10);
        builder.AddWeapons(20);
        builder.AddItems(10);
        builder.AddEnemies(5);
    }
    
    
    
    public void Generate(IDungeonThemeFactory? factory)
    {
        // if(_player)
        
        PlayerMovesBuilder pmb = new PlayerMovesBuilder(_pmBox);
        DungeonBuilder builder = new DungeonBuilder(this, _player, pmb);
        // OnlyMazeGenerate(builder);

        if (factory == null)
        {
            MixGenerate(builder);
            // ManySmallRoomsWithLootGenerate(builder);
            // CentralHallWithLootgenerate(builder);
            // builder.CreateFilledDungeon();
            builder.AddCentralHall(10,10);
        }
        else
        {
            factory.Build(builder, _player, this);
        }
        PickUpSoundBus.GetInstance.Init(this);

        Tiles = builder.GetDungeon();
        GetEnemies(); // TODO Zmien bo dosc bolesne ale nie chce zmieniac Tiles. 
        // var logger = Logger.GetInstance;
        // logger.Log("Generated enemies");
    }

    private void GetEnemies()
    {
        enemies = new();
        foreach (var tile in Tiles)
        {
            var x = tile.TryGetEnemy();
            if (x == null)
                continue;
            var logger = Logger.GetInstance;
            logger.Log($"- {x} - {tile.Content[0].Name}");
            enemies.Add(x);
        }
    }

    public BoardObject? GetCurrentlySeeked()
    {
        var tiles = Tiles[_player.Position[1], _player.Position[0]].Content;
        if (tiles == null || Tiles[_player.Position[1], _player.Position[0]].Objects <= _currentlySeeked)
            return null;
        return tiles[_currentlySeeked];
    }

    public void ResetSeek()
    {
        _currentlySeeked = 0;
    }

    public void TryIncreaseSeek()
    {
        if (_currentlySeeked < Tiles[_player.Position[1], _player.Position[0]].Objects - 1)
        {
            _currentlySeeked++;
        }
    }
    
    public void TryDecreaseSeek()
    {
        if (_currentlySeeked > 0)
            _currentlySeeked--;
    }

    public void AddPlayer(Player player, string? playerName = null)
    {
        _player = player;
        player.Name = playerName != null ? playerName : "Player 1";
            
        player.Position[0] = 1;
        player.Position[1] = 1;
    }
    
    public void MoveRight()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x + 1;
        int newY = y;
        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {Tiles[newY, newX].Content[0].Name} at {newX}, {newY}");
            EnemiesTurn();
            return;
        }
            

        _player.Position[0] = newX;
        _player.Position[1] = newY;

        
        DrawAt(x,y, GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        EnemiesTurn();
        _fightBox.AfterMoveAssesment(GetNearestEnemy());

    }
    public void MoveLeft()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x - 1;
        int newY = y;
        
        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {Tiles[newY, newX].Content[0].Name} at {newX}, {newY}");
            EnemiesTurn();
            return;
        }
            

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        DrawAt(x,y,GetVisualAt(x,y));
        

        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        EnemiesTurn();
        _fightBox.AfterMoveAssesment(GetNearestEnemy());
    }
    public void MoveDown()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y + 1;

        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {Tiles[newY, newX].Content[0].Name} at {newX}, {newY}");
            EnemiesTurn();
            return;
        }


        _player.Position[0] = newX;
        _player.Position[1] = newY;
        
        
        DrawAt(x,y,GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        EnemiesTurn();
        _fightBox.AfterMoveAssesment(GetNearestEnemy());
    }
    public void MoveUp()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y - 1;

        var logger = Logger.GetInstance;


        if (!CanMove(newX, newY))
        {
            logger.Log($"- {_player.Name} Tried To Walk into {Tiles[newY, newX].Content[0].Name} at {newX}, {newY}");
            EnemiesTurn();
            return;
        }

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        
        
        DrawAt(x,y,GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        EnemiesTurn();
        _fightBox.AfterMoveAssesment(GetNearestEnemy());
    }
    public void Display()
    {
        string[] signLines = GameConstants.AboveBoardSign.Split('\n');
        for (int i = 0; i < signLines.Length; i++)
        {
            Console.SetCursorPosition(GameConstants.SignStartLeft, GameConstants.SignStartTop + i);
            Console.Write(signLines[i]);
        }

        for (int y = 0; y < GameConstants.Height; y++)
        {
            Console.SetCursorPosition(GameConstants.BoardLeft, GameConstants.BoardTop + y);
            for (int x = 0; x < GameConstants.Width; x++)
            {
                Console.Write(GetVisualAt(x, y));
            }
        }

        string[] sign2Lines = GameConstants.BelowBoardSign.Split('\n');
        for (int i = 0; i < sign2Lines.Length; i++)
        {
            Console.SetCursorPosition(GameConstants.Sign2StartLeft, GameConstants.Sign2StartTop + i);
            Console.Write(sign2Lines[i]);
        }
    } 
 
    public void RefreshActionBox(int x, int y)
    {
        _actionBox.AfterMoveAsessment(Tiles[x,y].Content,Tiles[x,y].Objects); 
    }
    
    public void RefreshActionBox()
    {
        int x = _player.Position[1];
        int y = _player.Position[0];
        _actionBox.AfterMoveAsessment(Tiles[x,y].Content,Tiles[x,y].Objects, _currentlySeeked); 
    }

    public void DropItem(Tool tool)
    {
        int x = _player.Position[1];
        int y = _player.Position[0];
        tool.X = x;
        tool.Y = y;
        tool.ObjBoard = this;
        Tiles[x,y].AddObj(tool);
        DrawAt(y,x,GetVisualAt(y,x));
    }

    private bool IsEnemy(int x, int y)
    {
        if (Tiles[y, x].Objects > 0 && Tiles[y, x].Content[0].Fightable) // TODO WIEM WIEM WIEM KURCZe
            return true;
        return false;
    }

    public bool HasEnemiesNearby() // TODO - gra zakłada że jest jeden player - jeden enemy nearby
    {
        if (_player.Position[1] > 1 && IsEnemy(_player.Position[0], _player.Position[1] - 1))
            return true;
        if (_player.Position[1] < GameConstants.Height - 2 && IsEnemy(_player.Position[0], _player.Position[1] + 1))
            return true;
        if (_player.Position[0] > 0 && IsEnemy(_player.Position[0] - 1, _player.Position[1]))
            return true;
        if (_player.Position[0] < GameConstants.Width - 2 && IsEnemy(_player.Position[0] + 1, _player.Position[1]))
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

    public void FightNearestEnemy()
    {
        FightMode = true;
        
        if (_player.Position[1] > 1 && IsEnemy(_player.Position[0], _player.Position[1] - 1))
        {
            Enemy? e = Tiles[_player.Position[1] - 1, _player.Position[0]].TryGetEnemy();
            if (e == null)
                return;
            _menu = new FightMenu(_player,e, _fightBox, _game);
            var res = _menu.StartFight();
            if (!res) // killed enemy
            {
                return;
            }
            Tiles[_player.Position[1]-1, _player.Position[0]].Reset();
            DrawAt( _player.Position[0],_player.Position[1]-1, GetVisualAt(_player.Position[0],_player.Position[1]-1));
            return;
        }


        if (_player.Position[1] < GameConstants.Height - 2 && IsEnemy(_player.Position[0], _player.Position[1] + 1))
        {
            Enemy? e = Tiles[_player.Position[1] + 1, _player.Position[0]].TryGetEnemy();
            if (e == null)
                return;
            _menu = new FightMenu(_player,e, _fightBox,_game);
            var res = _menu.StartFight();
            if (!res) // killed enemy
            {
                return;
            }
            Tiles[_player.Position[1] + 1, _player.Position[0]].Reset();
            DrawAt( _player.Position[0],_player.Position[1] + 1, GetVisualAt( _player.Position[0],_player.Position[1] + 1));
            return;
        }

        if (_player.Position[0] > 0 && IsEnemy(_player.Position[0] - 1, _player.Position[1]))
        {
            Enemy? e = Tiles[_player.Position[1], _player.Position[0] - 1].TryGetEnemy();
            if (e == null)
                return;
            _menu = new FightMenu(_player,e, _fightBox,_game);
            var res = _menu.StartFight();
            if (!res) // killed enemy
            {
                return;
            }
            Tiles[_player.Position[1], _player.Position[0] - 1].Reset();
            DrawAt( _player.Position[0] - 1,_player.Position[1],GetVisualAt( _player.Position[0] - 1,_player.Position[1]));

            return;
            
        }

        if (_player.Position[0] < GameConstants.Width - 2 && IsEnemy(_player.Position[0] + 1, _player.Position[1]))
        {
            Enemy? e = Tiles[_player.Position[1], _player.Position[0] + 1].TryGetEnemy();
            if (e == null)
                return;
            _menu = new FightMenu(_player,e, _fightBox,_game);
            var res = _menu.StartFight();
            if (!res) // killed enemy
            {
                return;
            }
            Tiles[_player.Position[1], _player.Position[0] + 1].Reset();
            DrawAt( _player.Position[0] + 1, _player.Position[1],GetVisualAt( _player.Position[0] + 1,_player.Position[1]));
            return;
        }
        FightMode = false;
    }

    private void BlinkTile(int row, int column, ConsoleColor color, int durationMs, int blinkIntervalMs)
    {
        if (row < 0 || row >= Tiles.GetLength(0) || column < 0 || column >= Tiles.GetLength(1))
            return;

        _ = Task.Run(async () =>
        {
            int iterations = durationMs / blinkIntervalMs;

            for (int i = 0; i < iterations; i++)
            {
                DrawTile(row, column, i % 2 == 0 ? color : null);
                await Task.Delay(blinkIntervalMs);
            }

            DrawTile(row, column);
        });
    }

    public void Blink(int row, int column, ConsoleColor color)
    {
        BlinkTile(row, column, color, 3000, 250);
    }

    public void SoundBlink(int x, int y, ConsoleColor color)
    {
        if (y < 0 || y >= Tiles.GetLength(0) || x < 0 || x >= Tiles.GetLength(1))
            return;

        _ = Task.Run(async () =>
        {
            const int durationMs = 500;
            const int blinkIntervalMs = 125;
            int iterations = durationMs / blinkIntervalMs;
            bool isEmptyTile = Tiles[y, x].IsEmpty;

            for (int i = 0; i < iterations; i++)
            {
                if (i % 2 == 0)
                {
                    if (isEmptyTile && !(_player.Position[0] == x && _player.Position[1] == y))
                        ConsoleRender.WriteAt(GameConstants.BoardLeft + x, GameConstants.BoardTop + y, ' ', backgroundColor: color);
                    else
                        DrawAt(x, y, GetVisualAt(x, y), color);
                }
                else
                {
                    DrawAt(x, y, GetVisualAt(x, y));
                }

                await Task.Delay(blinkIntervalMs);
            }

            DrawAt(x, y, GetVisualAt(x, y));
        });
    }

    private void EnemyMove(Enemy e)
    {
        Random rnd = new Random();
        int x = rnd.Next(0, 100);
        if (x < 33)
            return;
        x = rnd.Next(0, 4);
        switch (x)
        {
            case 0:
            {
                if (e.X == 0 || !Tiles[e.X - 1, e.Y].IsEmpty)
                    return;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.X--;
                Tiles[e.X, e.Y].AddObj(e);
                DrawAt(oldColumn, oldRow, GetVisualAt(oldColumn, oldRow));
                DrawAt(e.Y, e.X, GetVisualAt(e.Y, e.X));
                break;
            }
            case 1:
            {
                if (e.Y == GameConstants.Width - 1 || !Tiles[e.X, e.Y + 1].IsEmpty)
                    return;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.Y++;
                Tiles[e.X, e.Y].AddObj(e);
                DrawAt(oldColumn, oldRow, GetVisualAt(oldColumn, oldRow));
                DrawAt(e.Y, e.X, GetVisualAt(e.Y, e.X));
                break;
            }
            case 2:
            {
                if (e.X == GameConstants.Height - 1 || !Tiles[e.X + 1, e.Y].IsEmpty)
                    return;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.X++;
                Tiles[e.X, e.Y].AddObj(e);
                DrawAt(oldColumn, oldRow, GetVisualAt(oldColumn, oldRow));
                DrawAt(e.Y, e.X, GetVisualAt(e.Y, e.X));
                break;
            }
            case 3:
            {
                if (e.Y == 0 || !Tiles[e.X, e.Y - 1].IsEmpty)
                    return;
                int oldRow = e.X;
                int oldColumn = e.Y;
                Tiles[oldRow, oldColumn].Reset();
                e.Y--;
                Tiles[e.X, e.Y].AddObj(e);
                DrawAt(oldColumn, oldRow, GetVisualAt(oldColumn, oldRow));
                DrawAt(e.Y, e.X, GetVisualAt(e.Y, e.X));
                break;
            }
        }

    }
    private void EnemiesTurn()
    {
        foreach (var enemy in enemies)
        {
            EnemyMove(enemy);
        }
    }

    public void DeleteEnemy(Enemy e)
    {
        enemies.Remove(e);
    }
    
    
}
