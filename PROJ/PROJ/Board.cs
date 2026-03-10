using PROJ.GameConstansts;
using PROJ.Goods.Classes.Valuables;
using PROJ.Tools.Classes.Items;
using PROJ.Tools.Classes.Weapons;

namespace PROJ;
using System;

public class Board
{
    private char[,] _state;

    public Tile[,] Tiles;
    
    private char _playerOvershadowedBox = ' ';
    
    private Player _player;

    private ActionBox _actionBox;

    private bool CanMove(int x, int y)
    {
        if (x >= GameConstants.Width-1 || x <= 0 || y >= GameConstants.Height-1 || y <= 0)
            return false;
        return _state[y, x] != GameConstants.WallSymbol;
    }

    private void DrawAt(int x, int y, char symbol)
    {
        Console.SetCursorPosition(GameConstants.BoardLeft + x, GameConstants.BoardTop + y);
        Console.Write(symbol);
    }
    private void FillInitialBoard()
    {
        
        _state = new char[GameConstants.Height, GameConstants.Width];
        Tiles = new Tile[GameConstants.Height, GameConstants.Width];
        for (int y = 0; y < GameConstants.Height; y++)
        {
            for (int x = 0; x < GameConstants.Width; x++)
            {
                Tiles[y, x] = new Tile();
                _state[y, x] = ' ';
            }
        }
        _state[0, 0] = '╔';
        _state[0, GameConstants.Width - 1] = '╗';
        _state[GameConstants.Height - 1, 0] = '╚';
        _state[GameConstants.Height - 1, GameConstants.Width - 1] = '╝';

        for (int i = 1; i < GameConstants.Width - 1; i++)
        {
            _state[0, i] = '═';
            _state[GameConstants.Height - 1, i] = '═';
        }
        for (int j = 1; j < GameConstants.Height-1; j++)
        {
            _state[j, 0] = '║';
            _state[j, GameConstants.Width - 1] = '║';
        }
    }
    private void AddObjToBoard(int x, int y, BoardObject obj) //TODO is it the best way to get coords ? 
    {
        obj.X = x;
        obj.Y = y;
        obj.ObjBoard = this;
        _state[x, y] = obj.Visual;
        DrawAt(y, x, obj.Visual);
        Tiles[x, y].Content = obj;
    }

    public void RemoveFromMap(int x, int y)
    {
        if (_player.Position[0] == x && _player.Position[1] == y)
        {
            _playerOvershadowedBox = ' ';
        }
        else
        {
            _state[x, y] = ' ';
        }
        Tiles[x,y].Reset();
    }
    public Board(ActionBox actionBox)
    {
        _actionBox = actionBox;
        FillInitialBoard();
    }

    public void AddPlayer(Player player)
    {
        _player = player;
        player.Position[0] = 1;
        player.Position[1] = 1;
        _state[1, 1] = GameConstants.PlayerSymbol;
    }

    
    public void MoveRight()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x + 1;
        int newY = y;

        if (!CanMove(newX, newY))
            return;

        _state[y, x] = _playerOvershadowedBox;
        DrawAt(x,y,_playerOvershadowedBox);

        _playerOvershadowedBox = _state[newY, newX];

        _player.Position[0] = newX;
        _player.Position[1] = newY;

        // Wstawienie gracza na nowe pole
        _state[newY, newX] = '¶';
        DrawAt(newX,newY,GameConstants.PlayerSymbol);
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content);
    }
    public void MoveLeft()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x - 1;
        int newY = y;

        if (!CanMove(newX, newY))
            return;

        _state[y, x] = _playerOvershadowedBox;
        DrawAt(x,y,_playerOvershadowedBox);

        _playerOvershadowedBox = _state[newY, newX];

        _player.Position[0] = newX;
        _player.Position[1] = newY;

        // Wstawienie gracza na nowe pole
        _state[newY, newX] = '¶';
        DrawAt(newX,newY,GameConstants.PlayerSymbol);
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content);
    }
    public void MoveDown()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y + 1;

        if (!CanMove(newX, newY))
            return;

        _state[y, x] = _playerOvershadowedBox;
        DrawAt(x,y,_playerOvershadowedBox);

        _playerOvershadowedBox = _state[newY, newX];

        _player.Position[0] = newX;
        _player.Position[1] = newY;

        // Wstawienie gracza na nowe pole
        _state[newY, newX] = '¶';
        DrawAt(newX,newY,GameConstants.PlayerSymbol);
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content);
    }
    public void MoveUp()
    {
        int x = _player.Position[0];
        int y = _player.Position[1];
        int newX = x;
        int newY = y - 1;

        if (!CanMove(newX, newY))
            return;

        _state[y, x] = _playerOvershadowedBox;
        DrawAt(x,y,_playerOvershadowedBox);

        _playerOvershadowedBox = _state[newY, newX];

        _player.Position[0] = newX;
        _player.Position[1] = newY;

        // Wstawienie gracza na nowe pole
        _state[newY, newX] = '¶';
        DrawAt(newX,newY,GameConstants.PlayerSymbol);
        _actionBox.AfterMoveAsessment(Tiles[_player.Position[1], _player.Position[0]].Content);
    }
    public void Display()
    {
        string[] signLines = GameConstants.AboveBoardSign.Split('\n');
        for (int i = 0; i < signLines.Length; i++) // 20+HEIGHT+2
        {
            Console.SetCursorPosition(GameConstants.SignStartLeft ,GameConstants.SignStartTop + i);
            Console.Write(signLines[i]);
        }
        Console.SetCursorPosition(0,GameConstants.SignStartTop);

        string[] sign2Lines = GameConstants.BelowBoardSign.Split('\n');
        for (int i = 0; i < GameConstants.Height; i++)
        {
            Console.SetCursorPosition(GameConstants.BoardLeft, GameConstants.BoardTop + i);
            for (int j = 0; j < GameConstants.Width; j++)
            {
                Console.Write(_state[i, j]);
            }

            // Console.WriteLine();
        }

        for (int i = 0; i < sign2Lines.Length; i++) // 20+HEIGHT+2
        {
            Console.SetCursorPosition(GameConstants.Sign2StartLeft ,GameConstants.Sign2StartTop + i);
            Console.Write(sign2Lines[i]);
        }
            
            
    }
    public void GenerateItems()
    {
        // itemki robimy
        Shiv shiv = new Shiv(_player);
        SailorsCutlass sc = new SailorsCutlass(_player);
        BoatHook bh = new BoatHook(_player);

        Physician_s_Ledger pl = new Physician_s_Ledger(_player);
        PlagueMask pm = new PlagueMask(_player);
        Rosary r = new Rosary(_player);


        AddObjToBoard(7, 10, shiv);
        AddObjToBoard(12, 34, sc);
        AddObjToBoard(1, 8, bh);
        AddObjToBoard(15,15,pl);
        AddObjToBoard(20,20,pm);
        AddObjToBoard(17,17,r);
        AddObjToBoard(10,10,new Coin());
        AddObjToBoard(11,11,new Coin());
        AddObjToBoard(13,13,new Gold());

    }

    

    public void GenerateWalls() // TODO pomyśl jak zrobić sciany aby nie blokowały przedmiotów/ odgradzały części planszy
    {
        Random rnd = new Random();
        for (int i = 1; i < 40; i++)
        {
            int x = rnd.Next(1,21);
            int y = rnd.Next(1,41);
            while (x == 1 && y == 1)
            {
                x = rnd.Next(1,21);
                y = rnd.Next(1,41);
            }
            _state[x,y] = '█'; // maybe pozmieniaj zeby sciany generowaly
                                                      // sie jakos z sensem a nie rozpieprzone
        }
    }
    
}