namespace PROJ.Tools.Classes.Items;

public class PlagueMask : Item
{
    public PlagueMask(Player player) : base(player){}
    public override char Visual => 'Ѫ';
    public override int Space => 1;
    public override string Name => "Plague Mask";
        
    
    public override void Use()
    {
    }
    public override bool Pickupable => Owner.Level > 5.0;
    public override string Description => "A leather beak mask meant to ward off sickness";
}