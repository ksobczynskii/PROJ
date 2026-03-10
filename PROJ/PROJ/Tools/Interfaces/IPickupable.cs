namespace PROJ.Tools;

public interface IPickupable
{
    bool Pickupable { get; }
    public void PickUp(Player player);
}