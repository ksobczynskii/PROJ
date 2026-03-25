namespace PROJ;

public class Wall : BoardObject
{
    public override string Name => "Wall";
    public override bool Pickupable => false;

    public override char Visual => '█';

    public override string Description => " ";

    public override void PickUp(Player player)
    {
        throw new Exception("Can't Pick up a wall");
    }
    
    public override bool Blocker => true;

}