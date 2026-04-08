using PROJ.Fight;
using PROJ.Fight.Interfaces;

namespace PROJ.Tools.Classes.Weapons.Abstract_Types;

public abstract class HeavyWeapon : Weapon
{
    public HeavyWeapon(Player player) : base(player)
    {
    }

    public override AttackResult Accept(IAttackVisitor visitor)
    {
        return visitor.VisitHeavyWeapon(this);
    }
}