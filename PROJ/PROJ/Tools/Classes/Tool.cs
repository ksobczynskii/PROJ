namespace PROJ.Tools.Classes;

public abstract class Tool :  BoardObject, ITool, IUsable // TODO use NuGet 
{
    public abstract int Space { get; }
    protected Player Owner;
    

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
            Owner.errSpace.DisplayErr("Can't Pick up that object yet!");
            return;
        }
        if (ObjBoard != null)
        {
            if (player.playerBackpack.TryAddItem(this))
            {
                ObjBoard.RemoveFromMap(X,Y);
                player.eqBox.DisplayItems();
                ObjBoard.RefreshActionBox(X,Y);
            }
            else
            {
                Owner.errSpace.DisplayErr("Item too large!");
            }
        }
    }

    public override bool Blocker => false;
}