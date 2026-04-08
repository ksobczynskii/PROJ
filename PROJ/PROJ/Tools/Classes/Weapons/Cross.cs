using PROJ.Tools.Classes.Weapons.Abstract_Types;

namespace PROJ.Tools.Classes.Weapons;

public class Cross : MagicWeapon
{
    public Cross(Player player) : base(player){}
    public override char Visual => '☦';
    public override int Space => 1;
    public override int Damage => 5;
    // public override int Range => 0;
    // public override float Cooldown => 1.0f;
    public override string Name => "Cross";

    public override void Use()
    {
    }

    public override bool TwoHanded() => false;

    public override string Description => "A wooden cross carried by the believers.";
}