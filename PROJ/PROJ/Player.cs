namespace PROJ;

public class Player
{
    // TODO zmien interfejs uzytkownika teraz, jak juz masz przedmioty
    public double Level;
    public int Speed;
    public int Health;
    public int Hunger;
    public int Strength;
    public int Wisdom;
    
    public string LeftHand;
    public string RightHand;
    
    private int[] _position; // 0 - x, 1 - y
    
    private Board _board;

    private Backpack _backpack;
    

    public int gold;
    public int coins;

    public WealthBox wBox;
    public Player(Board board)
    {
        Level = 1.0;
        Speed = 25;
        Health = 100;
        Hunger = 0;
        Strength = 25;
        Wisdom = 25;
        LeftHand = "";
        RightHand = "";
        _position = new int[2];
        _position[0] = 1;
        _position[1] = 1;
        _board = board;
        gold = 0;
        coins = 0;
        _backpack = new Backpack();
    }
    public int[] Position
    {
        get => _position;
        set => _position = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void TryPickUp()
    {
        if (_board.Tiles[Position[1], Position[0]].Content != null &&
            _board.Tiles[Position[1], Position[0]].Content.Pickupable) // TODO zbadaj to czm tu moze byc null niby
        {
            _board.Tiles[Position[1], Position[0]].Content.PickUp(this);
        }
    }

    public void UpdateWealth()
    {
        wBox.DisplayGoods();
    }
}