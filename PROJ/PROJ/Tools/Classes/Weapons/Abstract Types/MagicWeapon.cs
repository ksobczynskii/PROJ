using PROJ.Fight;
using PROJ.Fight.Interfaces;

namespace PROJ.Tools.Classes.Weapons.Abstract_Types;

public abstract class MagicWeapon : Weapon
{
    public MagicWeapon(Player player, string name = "unnamed", char vis = 'X') : base(player, name, vis)
    {
    }

    public override AttackResult Accept(IAttackVisitor visitor)
    {
        return visitor.VisitMagicWeapon(this);
    }
}
