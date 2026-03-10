namespace PROJ.Tools.Classes.Items;

public class Physician_s_Ledger : Item
{
    public Physician_s_Ledger(Player player) : base(player){}

    public override char Visual => '⎙';
    public override int Space => 1;
    public override string Name => "Physician's Ledger";
    public override void Use()
    {
    }
    public override bool Pickupable => Owner.Level > 10.0;
    public override string Description => "A doctor’s record book from the plague wards";
}