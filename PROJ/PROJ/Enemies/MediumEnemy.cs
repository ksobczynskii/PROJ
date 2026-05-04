namespace PROJ.Enemies;

public class MediumEnemy : Enemy
{
    public MediumEnemy(int health = 100, int armor = 5, int damage = 15,string name = "Infected", char vis = '☠') : base(health, armor, damage, vis, name)
    {
    }
    public override string Description => "Once a regular man turned zombie-like creature";
    public override void PickUp(Player player)
    {
        throw new NotImplementedException();
    }
}