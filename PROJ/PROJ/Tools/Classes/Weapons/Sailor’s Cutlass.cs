namespace PROJ.Tools.Classes.Weapons;

public class SailorsCutlass : Weapon
{
    public SailorsCutlass(Player player) : base(player){}
    public override char Visual => '☽';
    public override int Space => 1;
    public override int Damage => 10;
    public override int Range => 0;
    public override float Cooldown => 1.5f;
    public override string Name => "Sailor's Cutlass";


    public override void Use()
    {
    }

    public override string Description =>  "A sturdy cutlass once carried by a Marseille sailor.";
    public override bool TwoHanded() => false;
}