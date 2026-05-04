using PROJ.Tools.Classes;

namespace PROJ;

public class Player
{
    public double Level;
    public int Dexterity;
    public int Health;
    public int Luck;
    public int Strength;
    public int Wisdom;
    
    public Tool? LeftHand;
    public Tool? RightHand;
    
    private int[] _position; // 0 - x, 1 - y
    
    private Board _board;

    private VitalsBox _vitalsBox;
    

    public Backpack? PlayerBackpack;
    private bool _backPackMode;
    private int _backpackIdx;
    

    public int Gold;
    public int Coins;

    public WealthBox WBox;
    public EquipmentBox EqBox;
    public LeftHandBox LhBox;
    public RightHandBox RhBox;
    public AboveActionErrorSpace ErrSpace;
    public Player(Board board) // Tak jak w przypadku board - pewne fieldy nie moga byc nullami
    {
        Level = 1.0;
        Dexterity = 5;
        Health = 100;
        Luck = 5;
        Strength = 5;
        Wisdom = 5;
        LeftHand = null;
        RightHand = null;
        _position = new int[2];
        _position[0] = 1;
        _position[1] = 1;
        _board = board;
        Gold = 0;
        Coins = 0;
        PlayerBackpack = new Backpack();
        _backPackMode = false;
        _vitalsBox = new VitalsBox(this);
    }

    public VitalsBox GetVitalsBox => _vitalsBox;
    public int[] Position
    {
        get => _position;
        set => _position = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void TryPickUp()
    {
        var content = _board.GetCurrentlySeeked();
        if (content == null)
            return;
        
        content.PickUp(this);
    }

    public void TryIncrementBackpackIdx()
    {
        if (PlayerBackpack != null && _backpackIdx < PlayerBackpack.GetItemsCount ) // upsi
        {
            
            EqBox.PointerDown(_backpackIdx++);
        }
    }
    
    public void TryDecrementBackpackIdx()
    {
        if (PlayerBackpack != null && _backpackIdx > 0)
        {
            EqBox.PointerUp(_backpackIdx--);
        }
            
    }
    private void EnterBackpackMode()
    {
        _backPackMode = true;
        _backpackIdx = 0;
        EqBox.PointerInit();
    }
    
    private void ExitBackpackMode()
    {
        _backPackMode = false;
        EqBox.ClearPointer(_backpackIdx);
        _backpackIdx = 0;
    }
    public void SwitchBackpackMode()
    {
        if (!_backPackMode)
        {
            EnterBackpackMode();
        }
        else
        {
            ExitBackpackMode();
        }
    }
    public void UpdateWealth()
    {
        WBox.DisplayGoods();
    }
    
    public bool IsInBackpack
    {
        get => _backPackMode;
    }
    
    private void RightAdd()
    {
        if (PlayerBackpack == null)
            return;
        
        Tool t = PlayerBackpack.TryGetItem(_backpackIdx); // moze sie zmienic w trakcie
        if (t.Space > 2) // jak wyzej
            return;
        if (t.Space == 2)
        {
            if (LeftHand != null)
                return;
        }
        if (LeftHand != null && LeftHand.Space == 2)
            return;
        PlayerBackpack.Delete(_backpackIdx);
        EqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        EqBox.DisplayItems();
        _backpackIdx = 0;
        EqBox.PointerInit();
        RightHand = t;
        RhBox.DisplayHand();
    }
    private void LeftAdd()
    {
        if (PlayerBackpack == null)
            return;
        
        Tool t = PlayerBackpack.TryGetItem(_backpackIdx);
        if (t.Space > 2)
            return;
        if (t.Space == 2)
        {
            if (RightHand != null)
                return;
        }

        if (RightHand != null && RightHand.Space == 2)
            return;
        PlayerBackpack.Delete(_backpackIdx);
        EqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        EqBox.DisplayItems();
        _backpackIdx = 0;
        EqBox.PointerInit();
        LeftHand = t;
        LhBox.DisplayHand();
    }

    private void LeftSwap()
    {
        if (PlayerBackpack == null)
            return;
        Tool? t = PlayerBackpack.TryGetItem(_backpackIdx);
        if (t == null)
            return;
        if (LeftHand.Space == t.Space)
        {
            Tool newLeft = t;
            if (PlayerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
            {
                LeftHand = newLeft;
                LhBox.DisplayHand();
                EqBox.DisplayItemsLeavePointer();
            }
        }
        else if (LeftHand.Space > t.Space)
        {
            if (PlayerBackpack.IsEnoughCapForSwap(LeftHand.Space, t.Space))
            {
                Tool newLeft = t;
                if (PlayerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
                {
                    LeftHand = newLeft;
                    LhBox.DisplayHand();
                    EqBox.DisplayItemsLeavePointer();
                }
            }
        }
        else
        {
            if (t.Space == 2) // TODO na ten moment nie ma innych ale nie moge czegos o pojemnosci  > 2 wziac do reki na razie
            {
                if (RightHand != null)
                    return;
                Tool newLeft = t;
                if (PlayerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
                {
                    LeftHand = newLeft;
                    LhBox.DisplayHand();
                    EqBox.DisplayItemsLeavePointer();
                }
                
            }
        }
    }

    private void RightSwap()
    {
        if (PlayerBackpack == null)
            return;
        Tool? t = PlayerBackpack.TryGetItem(_backpackIdx);
        if (t == null)
            return;
        if (RightHand.Space == t.Space)
        {
            Tool newRight = t;
            if (PlayerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
            {
                RightHand = newRight;
                RhBox.DisplayHand();
                EqBox.DisplayItemsLeavePointer();
            }
        }
        else if (RightHand.Space > t.Space)
        {
            if (PlayerBackpack.IsEnoughCapForSwap(RightHand.Space, t.Space))
            {
                Tool newRight = t;
                if (PlayerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
                {
                    RightHand = newRight;
                    RhBox.DisplayHand();
                    EqBox.DisplayItemsLeavePointer();
                }
            }
        }
        else
        {
            if (t.Space == 2) // TODO na ten moment nie ma innych ale nie moge czegos o pojemnosci  > 2 wziac do reki na razie
            {
                if (LeftHand != null)
                    return;
                Tool newRight = t;
                if (PlayerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
                {
                    RightHand = newRight;
                    RhBox.DisplayHand();
                    EqBox.DisplayItemsLeavePointer();
                }
                
            }
        }
    }

    private void LeftAddToBp()
    {
        if (!PlayerBackpack.IsEnoughCapForSwap(LeftHand.Space, 0))
            return;
        Tool t = LeftHand;
        if (PlayerBackpack.TryAddItem(t))
        {
            LeftHand = null;
            LhBox.DisplayHand();
            EqBox.DisplayItemsLeavePointer();
        }
    }
    
    private void RightAddToBp()
    {
        if (!PlayerBackpack.IsEnoughCapForSwap(RightHand.Space, 0))
            return;
        Tool t = RightHand;
        if (PlayerBackpack.TryAddItem(t))
        {
            RightHand = null;
            RhBox.DisplayHand();
            EqBox.DisplayItemsLeavePointer();
        }
    }
    public void TrySwap(char c)
    {
        if (c == 'l')
        {
            if (LeftHand == null && PlayerBackpack.TryGetItem(_backpackIdx) == null)
                return;
            if (LeftHand == null)
                LeftAdd();
            else if(PlayerBackpack.TryGetItem(_backpackIdx) == null)
            {
                LeftAddToBp();
            }
            else
            {
                LeftSwap();
            }
        }
        else if (c == 'r')
        {
            if (RightHand == null && PlayerBackpack.TryGetItem(_backpackIdx) == null)
                return;
            if (RightHand == null)
                RightAdd();
            else if (PlayerBackpack.TryGetItem(_backpackIdx) == null)
            {
                RightAddToBp();
            }
            else
            {
                RightSwap();
            }
        }
    }

    public void BackpackDrop()
    {
        if (PlayerBackpack == null)
            return;
        Tool? tool = PlayerBackpack.Delete(_backpackIdx);
        if (tool == null)
        {
            return;
        }
        EqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        EqBox.DisplayItems();
        _backpackIdx = 0;
        EqBox.PointerInit();
        _board.DropItem(tool);
    }
    
    public bool Dead()
    {
        return Health == 0;
    }

    public void UpdateVitals()
    {
        _vitalsBox.DisplayVitals();
    }
    
    public string Name { get; set; }
}