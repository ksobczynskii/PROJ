namespace PROJ.Tools.Classes.Decorators;

public class StrongWeaponDecorator : WeaponDecorator
{
    public override string Name => _inner.Name + " (Strong)";
    public override int GetStrength => 5;

    public StrongWeaponDecorator(Player player, Weapon weapon) : base(player, weapon)
    {
    }
}