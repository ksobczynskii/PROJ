namespace PROJ.Tools.Classes;

public abstract class Weapon : Tool
{
    public abstract int Damage { get; }
    public abstract float Cooldown { get; }
    public abstract int Range { get; }


    public Weapon(Player player) : base(player){}

    
    public abstract bool TwoHanded();
}