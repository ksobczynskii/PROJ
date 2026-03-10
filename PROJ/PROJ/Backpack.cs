using PROJ.GameConstansts;
using PROJ.Tools.Classes;

namespace PROJ;

public class Backpack
{
    private Tool[] _inventory;
    private int _items;
    public Backpack()
    {
        _inventory = new Tool[GameConstants.BackpackCapacity];
        _items = 0;
    }

    public bool TryAddItem(Item item)
    {
        if (_items < GameConstants.BackpackCapacity)
        {
            _inventory[_items++] = item;
            return true;
        }

        return false;
    }
    
}