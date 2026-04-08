using PROJ.Builder.Classes;
using PROJ.Enemies;
using PROJ.Fight;
using PROJ.GameConstansts;
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

    private void DrawAt(int x, int y, char symbol)
    {
        Console.SetCursorPosition(GameConstants.BoardLeft + x, GameConstants.BoardTop + y);
        Console.Write(symbol);
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
    
    
    
    public void Generate()
    {
        // if(_player)
        PlayerMovesBuilder pmb = new PlayerMovesBuilder(_pmBox);
        DungeonBuilder builder = new DungeonBuilder(this, _player, pmb);
        // OnlyMazeGenerate(builder);
        MixGenerate(builder);
        // ManySmallRoomsWithLootGenerate(builder);
        // CentralHallWithLootgenerate(builder);
        // builder.CreateFilledDungeon();
        builder.AddCentralHall(10,10);
        Tiles = builder.GetDungeon();
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

    public void AddPlayer(Player player)
    {
        _player = player;
        player.Position[0] = 1;
        player.Position[1] = 1;
    }
    
    public void MoveRight()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x + 1;
        int newY = y;

        if (!CanMove(newX, newY))
            return;

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        
        DrawAt(x,y, GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        _fightBox.AfterMoveAssesment(GetNearestEnemy());

    }
    public void MoveLeft()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x - 1;
        int newY = y;

        if (!CanMove(newX, newY))
            return;

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        DrawAt(x,y,GetVisualAt(x,y));

        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        _fightBox.AfterMoveAssesment(GetNearestEnemy());
    }
    public void MoveDown()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y + 1;

        if (!CanMove(newX, newY))
            return;


        _player.Position[0] = newX;
        _player.Position[1] = newY;
        DrawAt(x,y,GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
        _fightBox.AfterMoveAssesment(GetNearestEnemy());
    }
    public void MoveUp()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y - 1;

        if (!CanMove(newX, newY))
            return;

        _player.Position[0] = newX;
        _player.Position[1] = newY;
        DrawAt(x,y,GetVisualAt(x,y));
        DrawAt(newX,newY,GetVisualAt(newX, newY));
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content,Tiles[_player.Position[1], _player.Position[0]].Objects );
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
    
    
}