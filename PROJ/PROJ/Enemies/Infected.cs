namespace PROJ.Enemies;

public class Infected : Enemy
{
    public Infected(int health = 100, int armor = 5, int damage = 15)
    {
        _health = health;
        _armor = armor;
        _damage = damage;
    }
    public override char Visual => '☠';
    public override string Name => "Infected";
    public override string Description => "Once a regular man turned zombie-like creature";
    public override void PickUp(Player player)
    {
        throw new NotImplementedException();
    }
}