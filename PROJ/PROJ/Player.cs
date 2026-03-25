using PROJ.Tools.Classes;

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
    
    public Tool? LeftHand;
    public Tool? RightHand;
    
    private int[] _position; // 0 - x, 1 - y
    
    private Board _board;
    

    public Backpack? playerBackpack;
    private bool _backPackMode;
    private int _backpackIdx;
    

    public int gold;
    public int coins;

    public WealthBox wBox;
    public EquipmentBox eqBox;
    public LeftHandBox lhBox;
    public RightHandBox rhBox;
    public AboveActionErrorSpace errSpace;
    public Player(Board board)
    {
        Level = 1.0;
        Speed = 25;
        Health = 100;
        Hunger = 0;
        Strength = 25;
        Wisdom = 25;
        LeftHand = null;
        RightHand = null;
        _position = new int[2];
        _position[0] = 1;
        _position[1] = 1;
        _board = board;
        gold = 0;
        coins = 0;
        playerBackpack = new Backpack();
        _backPackMode = false;
    }
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
        if (playerBackpack != null && _backpackIdx < playerBackpack.GetItemsCount ) // upsi
        {
            
            eqBox.PointerDown(_backpackIdx++);
        }
    }
    
    public void TryDecrementBackpackIdx()
    {
        if (playerBackpack != null && _backpackIdx > 0)
        {
            eqBox.PointerUp(_backpackIdx--);
        }
            
    }
    private void EnterBackpackMode()
    {
        _backPackMode = true;
        _backpackIdx = 0;
        eqBox.PointerInit();
    }
    
    private void ExitBackpackMode()
    {
        _backPackMode = false;
        eqBox.ClearPointer(_backpackIdx);
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
        wBox.DisplayGoods();
    }
    
    public bool IsInBackpack
    {
        get => _backPackMode;
    }
    
    public int GetBackpackIdx
    {
        get => _backpackIdx;
        
    }
    
    private void RightAdd()
    {
        if (playerBackpack == null)
            return;
        
        Tool t = playerBackpack.TryGetItem(_backpackIdx);
        if (t.Space > 2)
            return;
        if (t.Space == 2)
        {
            if (LeftHand != null)
                return;
        }
        if (LeftHand != null && LeftHand.Space == 2)
            return;
        playerBackpack.Delete(_backpackIdx);
        eqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        eqBox.DisplayItems();
        _backpackIdx = 0;
        eqBox.PointerInit();
        RightHand = t;
        rhBox.DisplayHand();
    }
    private void LeftAdd()
    {
        if (playerBackpack == null)
            return;
        
        Tool t = playerBackpack.TryGetItem(_backpackIdx);
        if (t.Space > 2)
            return;
        if (t.Space == 2)
        {
            if (RightHand != null)
                return;
        }

        if (RightHand != null && RightHand.Space == 2)
            return;
        playerBackpack.Delete(_backpackIdx);
        eqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        eqBox.DisplayItems();
        _backpackIdx = 0;
        eqBox.PointerInit();
        LeftHand = t;
        lhBox.DisplayHand();
    }

    private void LeftSwap()
    {
        if (playerBackpack == null)
            return;
        Tool? t = playerBackpack.TryGetItem(_backpackIdx);
        if (t == null)
            return;
        if (LeftHand.Space == t.Space)
        {
            Tool newLeft = t;
            if (playerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
            {
                LeftHand = newLeft;
                lhBox.DisplayHand();
                eqBox.DisplayItemsLeavePointer();
            }
        }
        else if (LeftHand.Space > t.Space)
        {
            if (playerBackpack.IsEnoughCapForSwap(LeftHand.Space, t.Space))
            {
                Tool newLeft = t;
                if (playerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
                {
                    LeftHand = newLeft;
                    lhBox.DisplayHand();
                    eqBox.DisplayItemsLeavePointer();
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
                if (playerBackpack.TryOverwriteItemAt(LeftHand, _backpackIdx))
                {
                    LeftHand = newLeft;
                    lhBox.DisplayHand();
                    eqBox.DisplayItemsLeavePointer();
                }
                
            }
        }
    }

    private void RightSwap()
    {
        if (playerBackpack == null)
            return;
        Tool? t = playerBackpack.TryGetItem(_backpackIdx);
        if (t == null)
            return;
        if (RightHand.Space == t.Space)
        {
            Tool newRight = t;
            if (playerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
            {
                RightHand = newRight;
                rhBox.DisplayHand();
                eqBox.DisplayItemsLeavePointer();
            }
        }
        else if (RightHand.Space > t.Space)
        {
            if (playerBackpack.IsEnoughCapForSwap(RightHand.Space, t.Space))
            {
                Tool newRight = t;
                if (playerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
                {
                    RightHand = newRight;
                    rhBox.DisplayHand();
                    eqBox.DisplayItemsLeavePointer();
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
                if (playerBackpack.TryOverwriteItemAt(RightHand, _backpackIdx))
                {
                    RightHand = newRight;
                    rhBox.DisplayHand();
                    eqBox.DisplayItemsLeavePointer();
                }
                
            }
        }
    }

    private void LeftAddToBp()
    {
        if (!playerBackpack.IsEnoughCapForSwap(LeftHand.Space, 0))
            return;
        Tool t = LeftHand;
        if (playerBackpack.TryAddItem(t))
        {
            LeftHand = null;
            lhBox.DisplayHand();
            eqBox.DisplayItemsLeavePointer();
        }
    }
    
    private void RightAddToBp()
    {
        if (!playerBackpack.IsEnoughCapForSwap(RightHand.Space, 0))
            return;
        Tool t = RightHand;
        if (playerBackpack.TryAddItem(t))
        {
            RightHand = null;
            rhBox.DisplayHand();
            eqBox.DisplayItemsLeavePointer();
        }
    }
    public void TrySwap(char c)
    {
        if (c == 'l')
        {
            if (LeftHand == null && playerBackpack.TryGetItem(_backpackIdx) == null)
                return;
            if (LeftHand == null)
                LeftAdd();
            else if(playerBackpack.TryGetItem(_backpackIdx) == null)
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
            if (RightHand == null && playerBackpack.TryGetItem(_backpackIdx) == null)
                return;
            if (RightHand == null)
                RightAdd();
            else if (playerBackpack.TryGetItem(_backpackIdx) == null)
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
        if (playerBackpack == null)
            return;
        Tool? tool = playerBackpack.Delete(_backpackIdx);
        if (tool == null)
        {
            return;
        }
        eqBox.ClearPointer(_backpackIdx); // TODO - w przyszlosci zmieniamy in place
        eqBox.DisplayItems();
        _backpackIdx = 0;
        eqBox.PointerInit();
        _board.DropItem(tool);
    }
    
}