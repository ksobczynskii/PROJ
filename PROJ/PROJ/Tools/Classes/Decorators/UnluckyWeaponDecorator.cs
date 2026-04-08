namespace PROJ.Tools.Classes.Decorators;

public class UnluckyWeaponDecorator : WeaponDecorator
{
    public override string Name => _inner.Name + " (Unlucky)";
    public override int GetLuck => -5;

    public UnluckyWeaponDecorator(Player player, Weapon weapon) : base(player, weapon)
    {
    }
}