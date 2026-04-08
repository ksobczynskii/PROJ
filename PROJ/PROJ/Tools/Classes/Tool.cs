using PROJ.Fight;
using PROJ.Fight.Interfaces;
using PROJ.GameConstansts;

namespace PROJ.Tools.Classes;

public abstract class Tool :  BoardObject, ITool, IUsable // TODO use NuGet 
{
    public abstract int Space { get; }
    protected Player Owner;

    public override bool Fightable => false;


    public Tool(Player player)
    {
        Owner = player;
    }

    public abstract void Use();
    public override bool Pickupable => true; // TODO musze sprawdzic czy ta logika zadziała

    public override void PickUp(Player player)
    {
        if (!Pickupable)
        {
            Owner.ErrSpace.DisplayErr("Can't Pick up that object yet!");
            return;
        }
        if (ObjBoard != null)
        {
            if (player.PlayerBackpack.TryAddItem(this))
            {
                ObjBoard.RemoveFromMap(X,Y);
                player.EqBox.DisplayItems();
                ObjBoard.RefreshActionBox(X,Y);
            }
            else
            {
                Owner.ErrSpace.DisplayErr("Item too large!");
            }
        }
    }
    public virtual int GetStrength => 0;

    public virtual int GetLuck => 0;
    public virtual int GetDexterity => 0;
    public virtual int GetWisdom => 0;

    public override bool Blocker => false;
    
    public virtual AttackResult Accept(IAttackVisitor visitor)
    {
        return visitor.VisitOther();
    }
}