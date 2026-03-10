namespace PROJ.Tools.Classes;

public abstract class Tool :  BoardObject, ITool, IUsable
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
        //TODO player picks up obj
    }
    
}