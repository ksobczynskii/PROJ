using PROJ.Builder.Classes;
using PROJ.GameConstansts;
using PROJ.Goods.Classes.Valuables;
using PROJ.Tools.Classes;
using PROJ.Tools.Classes.Items;
using PROJ.Tools.Classes.Weapons;

namespace PROJ;
using System;

public class Board
{

    public Tile[,] Tiles;
    
    private Player _player;

    private ActionBox _actionBox;
    
    private int _currentlySeeked;
    private PlayerMovesBox _pmBox;
    


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
        if (Tiles[y, x].Content != null && Tiles[y, x].Objects > 0)
            return Tiles[y,x].Content[0].Visual; // TODO pusibul nul

        return ' ';
    }

    private void DrawAt(int x, int y, char symbol)
    {
        Console.SetCursorPosition(GameConstants.BoardLeft + x, GameConstants.BoardTop + y);
        Console.Write(symbol);
    }
    private void AddObjToBoard(int x, int y, BoardObject obj) //TODO what if i add to user space?
    {
        obj.X = x;
        obj.Y = y;
        obj.ObjBoard = this;
        Tiles[x, y].AddObj(obj);
        DrawAt(y, x, GetVisualAt(y,x));
        
    }

    public void RemoveFromMap(int x, int y)
    {
        Tiles[x,y].Remove(_currentlySeeked);
        DrawAt(y,x,GetVisualAt(y,x));
        
    }
    public Board(ActionBox actionBox, PlayerMovesBox pmBox)
    {
        _currentlySeeked = 0;
        _actionBox = actionBox;
        _pmBox = pmBox;
    }

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
        builder.AddCorridors();
    }

    private void MixGenerate(DungeonBuilder builder)
    {
        builder.CreateEmptyDungeon();
        builder.AddCentralHall(10,7);
        builder.AddRooms(4);
        builder.AddCorridors();
        builder.AddWeapons(30);
        builder.AddItems(20);
    }
    
    
    
    public void Generate()
    {
        PlayerMovesBuilder pmb = new PlayerMovesBuilder(_pmBox);
        DungeonBuilder builder = new DungeonBuilder(this, _player, pmb);
        // OnlyMazeGenerate(builder);
        // MixGenerate(builder);
        // ManySmallRoomsWithLootGenerate(builder);
        CentralHallWithLootgenerate(builder);
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
    public void GenerateItems()
    {
        // itemki robimy
        Shiv shiv = new Shiv(_player);
        SailorsCutlass sc = new SailorsCutlass(_player);
        BoatHook bh = new BoatHook(_player);
        BoatHook bh1 = new BoatHook(_player);
        SailorsCutlass sc1 = new SailorsCutlass(_player);
        
        

        Physician_s_Ledger pl = new Physician_s_Ledger(_player);
        PlagueMask pm = new PlagueMask(_player);
        Rosary r = new Rosary(_player);


        // for (int i = 2; i < GameConstants.Width - 1; i++)
        // {
        //     AddObjToBoard(8,i,new Wall());
        // }


        AddObjToBoard(7, 10, shiv);
        AddObjToBoard(7, 10, bh1);
        AddObjToBoard(7, 10, sc1);
        AddObjToBoard(12, 34, sc);
        AddObjToBoard(1, 8, bh);
        AddObjToBoard(15,15,pl);
        AddObjToBoard(20,20,pm);
        AddObjToBoard(17,17,r);
        AddObjToBoard(10,10,new Coin());
        AddObjToBoard(11,11,new Coin());
        AddObjToBoard(13,13,new Gold());

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
    
}