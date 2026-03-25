using PROJ.DSA;
using PROJ.GameConstansts;
using PROJ.Tools.Classes;
using PROJ.Tools.Classes.Items;
using PROJ.Tools.Classes.Weapons;

namespace PROJ.Builder.Classes;

public class DungeonBuilder : IDungeonBuilder
{
    struct Room
    {
        public int left;
        public int top;
        public int width;
        public int height;
        public bool connected;
    }
    
    
    private Tile[,] _tiles;
    private Board _board;
    private List<Room> _rooms;
    private bool _playerRoom;
    private bool _corridors;
    private Player _player;
    private PlayerMovesBuilder _pmb;
    

    
    
    public DungeonBuilder(Board b, Player p, PlayerMovesBuilder pmb)
    {
        _pmb = pmb;
        _player = p;
        _corridors = false;
        _playerRoom = false;
        _board = b;
        _rooms = new List<Room>();
        _tiles = new Tile[GameConstants.Height, GameConstants.Width];
        for (int y = 0; y < GameConstants.Height; y++)
        {
            for (int x = 0; x < GameConstants.Width; x++)
            {
                _tiles[y, x] = new Tile();
            }
        }

        _tiles[0,0] = new Tile();
        _tiles[0, GameConstants.Width - 1] = new Tile();
        _tiles[GameConstants.Height - 1, 0] = new Tile();
        _tiles[GameConstants.Height - 1, GameConstants.Width - 1] = new Tile();
        
        
        _tiles[0,0].AddObj(new FrameObject('╔'));
        _tiles[0, GameConstants.Width - 1].AddObj(new FrameObject( '╗'));
        _tiles[GameConstants.Height - 1, 0].AddObj(new FrameObject('╚'));
        _tiles[GameConstants.Height - 1, GameConstants.Width - 1].AddObj(new FrameObject('╝'));
        

        

        for (int i = 1; i < GameConstants.Width - 1; i++)
        {

            _tiles[0, i] = new Tile();
            _tiles[GameConstants.Height - 1, i] = new Tile();
            
            _tiles[0,i].AddObj(new FrameObject('═'));
            _tiles[GameConstants.Height - 1,i].AddObj(new FrameObject('═'));
            
        }
        for (int j = 1; j < GameConstants.Height-1; j++)
        {

            _tiles[j, 0] = new Tile();;
            _tiles[j, GameConstants.Width - 1] = new Tile();
            
            _tiles[j,0].AddObj(new FrameObject('║'));
            _tiles[j, GameConstants.Width - 1].AddObj(new FrameObject('║'));
        }
    }

    public void CreateEmptyDungeon()
    {
        _pmb.AddInitial();
        for (int i = 1; i < GameConstants.Width-1; i++)
        {
            for (int j = 1; j < GameConstants.Height-1; j++)
            {
                _tiles[j, i] = new Tile();
            }
        }
    }

    public void CreateFilledDungeon()
    {
        _pmb.AddInitial();
        for (int i = 1; i < GameConstants.Width-1; i++)
        {
            for (int j = 1; j < GameConstants.Height-1; j++)
            {
                _tiles[j, i] = new Tile();
                _tiles[j,i].AddObj(new Wall());
            }
        }
        _tiles[1,1].Reset();
    }

    public void AddCentralHall(int width, int height)
    {
        if (width >= GameConstants.Width - 2 || height >= GameConstants.Height - 2)
            return;

        int left = (GameConstants.Width - width) / 2;
        int right = left + width - 1;

        int top = (GameConstants.Height - height) / 2;
        int bottom = top + height - 1;
        if (DoRoomsOverlap(left, top, width, height))
            return;

        for (int x = left; x <= right; x++)
        {
            _tiles[top, x].Reset();
            _tiles[top, x].AddObj(new Wall());

            _tiles[bottom, x].Reset();
            _tiles[bottom, x].AddObj(new Wall());
        }

        for (int y = top; y <= bottom; y++)
        {
            _tiles[y, left].Reset();
            _tiles[y, left].AddObj(new Wall());

            _tiles[y, right].Reset();
            _tiles[y, right].AddObj(new Wall());
        }

        for (int x = left + 1; x < right; x++)
        {
            for (int y = top + 1; y < bottom; y++)
            {
                _tiles[y, x].Reset();
            }
        }

        Room r = new Room() { top = top, height = height, width = width, left = left, connected = false };
        _rooms.Add(r);
        OpenRooms();
        
    }

    private bool DoRoomsOverlap(int left, int top, int width, int height)
    {
        int newLeft = left;
        int newRight = left + width - 1;
        int newTop = top;
        int newBottom = top + height - 1;

        foreach (var room in _rooms)
        {
            int roomLeft = room.left;
            int roomRight = room.left + room.width - 1;
            int roomTop = room.top;
            int roomBottom = room.top + room.height - 1;

            bool noOverlap =
                newRight < roomLeft ||
                newLeft > roomRight ||
                newBottom < roomTop ||
                newTop > roomBottom;
            if (!noOverlap)
                return true;
        }
        
        
        
        return false;
    }

    private void DrawRoom(Room room)
    {
        int left = room.left;
        int right = room.left + room.width - 1;
        int top = room.top;
        int bottom = room.top + room.height - 1;

        for (int x = left; x <= right; x++)
        {
            if (!_tiles[top, x].BlocksMovement)
            {
                _tiles[top, x].Reset();
                _tiles[top, x].AddObj(new Wall());
            }

            if (!_tiles[bottom, x].BlocksMovement)
            {
                _tiles[bottom, x].Reset();
                _tiles[bottom, x].AddObj(new Wall());
            }
            
        }

        for (int y = top; y <= bottom; y++)
        {
            if (!_tiles[y, left].BlocksMovement)
            {
                _tiles[y, left].Reset();
                _tiles[y, left].AddObj(new Wall());
            }

            if (!_tiles[y, right].BlocksMovement)
            {
                _tiles[y, right].Reset();
                _tiles[y, right].AddObj(new Wall());
            }
        }

        for (int y = top + 1; y < bottom; y++)
        {
            for (int x = left + 1; x < right; x++)
            {
                _tiles[y, x].Reset();
            }
        }
    }
    private bool AddRoom()
    {
        Random r = new Random();
        int width = 0;
        int height = 0;
        int left = 0;
        int top = 0;
        int i = 0;
        for (i = 0; i < GameConstants.RoomInsertAttempts; i++)
        {
            width = r.Next() % (GameConstants.Width / 4) + 3;
            height = r.Next() % (GameConstants.Height / 4) + 3;
            left = r.Next() % (GameConstants.Width - width - 2) + 2;
            top = r.Next() % (GameConstants.Height - height - 2) + 2;
            
            if (!DoRoomsOverlap(left, top, width, height))
                break;
        }

        if (i == GameConstants.RoomInsertAttempts)
            return false;

        if (width == 0)
            return false;
        Room room = new Room() { height = height, width = width, left = left, top = top, connected = false};
        
        _rooms.Add(room);
        DrawRoom(room);
        return true;

    }

    private bool AddPlayerRoom() // Funkcja zakłada gracza w (1,1)
    {
        Random r = new Random();
        int width = 0;
        int height = 0;
        int i = 0;
        for (i = 0; i < GameConstants.RoomInsertAttempts; i++)
        {
            width = r.Next() % (GameConstants.Width / 4) + 3;
            height = r.Next() % (GameConstants.Height / 4) + 3;
            if (!DoRoomsOverlap(1, 1, width, height))
                break;
        }
        if (i == GameConstants.RoomInsertAttempts)
            return false;
        if (width == 0)
            return false;
        Room room = new Room() { height = height, width = width, left = 0, top = 0, connected = false};
        _rooms.Add(room);
        DrawRoom(room);
        _playerRoom = true;
        return true;
    }
    public void AddRooms(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            if(!_playerRoom)
                AddPlayerRoom();
            else
            {
                AddRoom();
            }
        }
        // AddCorridors();
        OpenRooms();
    }

    private bool IsInRoom(int i, int j)
    {
        foreach (var room in _rooms)
        {
            if (j >= room.left &&
                j <= room.left + room.width - 1 &&
                i >= room.top &&
                i <= room.top + room.height - 1)
                return true;
        }

        return false;
    }
    private void FillTile(int i, int j, bool Wall, UnionFind<(int i, int j)> uf, PriorityQueue<(int i, int j), int> pq, Random r)
    {
        if (IsInRoom(i, j))
            return;
        _tiles[i, j].Reset();
        if (Wall)
        {
            _tiles[i, j].AddObj(new Wall());
            pq.Enqueue((i, j), r.Next(100));
        }
        else
        {
            uf.Add((i,j));
        }
    }
    private void PreKruskalGeneration(UnionFind<(int i, int j)> uf, PriorityQueue<(int i, int j), int> pq)
    {
        Random r = new Random();
        bool Wall = false;
        for (int i = 1; i < GameConstants.Height-1; i++)
        {
            for (int j = 1; j < GameConstants.Width - 1; j++)
            {
                if (j % 2 == 0)
                    FillTile(i,j,Wall, uf, pq, r);
                else
                {
                    FillTile(i, j, !Wall, uf, pq, r);
                }
            }
            Wall = !Wall;
        }
    }


    private bool AreAlmostDisjoint(int s1, int s2, int s3, int s4)
    {
        int[] values = { s1, s2, s3, s4 };
        int times = 0;
        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[i] == values[j] && values[i] != -1)
                    times++;
            }
        }

        return times < 2;
    }
    private bool AreDisjoint(int s1, int s2, int s3, int s4)
    {
        int[] values = { s1, s2, s3, s4 };

        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[i] == values[j] && values[i] != -1)
                    return false;
            }
        }

        return true;
    }
    
    private bool AnyDisjoint(int s1, int s2, int s3, int s4)
    {
        int[] values = { s1, s2, s3, s4 };

        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[i] != values[j] && values[i] != -1)
                    return true;
            }
        }

        return false;
    }
    private void Kruskal(UnionFind<(int i, int j)> uf, PriorityQueue<(int i, int j), int> pq)
    {
        while (pq.Count > 0)
        {
            (int i, int j) = pq.Dequeue();
            if (AreAlmostDisjoint(uf.Find((i - 1, j)), uf.Find((i + 1, j)), uf.Find((i, j - 1)), uf.Find((i, j + 1))))
            {
                uf.Union((i - 1, j), (i + 1, j));
                uf.Union((i - 1, j), (i, j-1));
                uf.Union((i - 1, j), (i, j+1));
                uf.Add((i,j));
                uf.Union((i - 1, j), (i, j));
                _tiles[i,j].Reset();
            }
        }
    }

    private bool IsInside(int i, int j)
    {
        return i >= 0 && i < GameConstants.Height &&
               j >= 0 && j < GameConstants.Width;
    }

    private bool IsEmptyTile(int i, int j)
    {
        return IsInside(i, j) && !_tiles[i, j].BlocksMovement;
    }

    private void OpenRooms()
    {
        Random _rng = new Random();
        foreach (var room in _rooms)
        {
            List<(int i, int j)> candidates = new List<(int i, int j)>();

            int left = room.left;
            int right = room.left + room.width - 1;
            int top = room.top;
            int bottom = room.top + room.height - 1;

            for (int j = left + 1; j < right; j++)
            {
                if (IsEmptyTile(top - 1, j))
                    candidates.Add((top, j));
            }

            for (int j = left + 1; j < right; j++)
            {
                if (IsEmptyTile(bottom + 1, j))
                    candidates.Add((bottom, j));
            }

            for (int i = top + 1; i < bottom; i++)
            {
                if (IsEmptyTile(i, left - 1))
                    candidates.Add((i, left));
            }

            for (int i = top + 1; i < bottom; i++)
            {
                if (IsEmptyTile(i, right + 1))
                    candidates.Add((i, right));
            }

            if (candidates.Count > 0)
            {
                var hole = candidates[_rng.Next(candidates.Count)];
                _tiles[hole.i, hole.j].Reset();
            }
        }
    }
    public void AddCorridors() // TODO Change
    {
        UnionFind<(int i, int j)> uf = new UnionFind<(int i, int j)>();
        PriorityQueue<(int i, int j), int> pq = new PriorityQueue<(int i, int j), int>();
        PreKruskalGeneration(uf, pq);
        Kruskal(uf, pq);
        OpenRooms();
    }
    private void AddObjToBoard(int x, int y, BoardObject obj)
    {
        obj.X = x;
        obj.Y = y;
        obj.ObjBoard = _board;
        _tiles[x, y].AddObj(obj);
        
    }
    private bool AddToMap(BoardObject obj)
    {
        Random r = new Random();
        if (_rooms.Count > 0)
        {
            Room room = _rooms[r.Next(_rooms.Count)];

            int left = room.left;
            int right = room.left + room.width - 1;
            int top = room.top;
            int bottom = room.top + room.height - 1;

            int j = r.Next(left + 1, right);
            int i = r.Next(top + 1, bottom);

            AddObjToBoard(i,j,obj);
            return true;
        }

        for (int attempt = 0; attempt < 100; attempt++)
        {
            int i = r.Next(1, GameConstants.Height - 1);
            int j = r.Next(1, GameConstants.Width - 1);

            if (_tiles[i, j].BlocksMovement)
                continue;

            AddObjToBoard(i,j,obj);
            return true;
        }

        return false;
    }
    public void AddItems(int count)
    {
        var items = new Func<Item>[]
        {
            () => new Physician_s_Ledger(_player),
            () => new PlagueMask(_player),
            () => new Rosary(_player)
        };
        Random r = new Random();
        // _tiles[1,2] = items[r.Next(items.Length)]();
        for (int i = 0; i < count; i++)
        {
            AddToMap(items[r.Next(items.Length)]());
        }
    }

    public void AddWeapons(int count)
    {
        _pmb.AddPickup();
        var weapons = new Func<Tool>[]
        {
            () => new Shiv(_player),
            () => new SailorsCutlass(_player),
            () => new BoatHook(_player)
        };
        Random r = new Random();
        var s = new Shiv(_player);
        // _tiles[1, 2].AddObj(s);
        for (int i = 0; i < count; i++)
        {
            AddToMap(weapons[r.Next(weapons.Length)]());
        }
    }

    public Tile[,] GetDungeon()
    {
        return _tiles;
    }
}