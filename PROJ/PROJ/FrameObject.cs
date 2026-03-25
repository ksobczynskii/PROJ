namespace PROJ;

public class FrameObject : BoardObject
{
    private char _vis;
    public FrameObject(char c)
    {
        _vis = c;
    }

    public override char Visual => _vis;
    public override bool Blocker => true;
    public override string Description => " ";

    public override string Name => "";

    public override void PickUp(Player player)
    {
    }
    
    public override bool Pickupable => false;
}