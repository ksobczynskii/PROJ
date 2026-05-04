namespace PROJ.Enemies;

public class SmallEnemy : Enemy
{
    public SmallEnemy(int health = 30, int armor = 5, int damage = 15,string name = "Rat", char vis = '~') : base(health, armor, damage, vis, name)
    {
    }

    public override string Description =>
        "Small creature, but one bite might get you infected.";
}