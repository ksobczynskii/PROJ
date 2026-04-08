namespace PROJ.Enemies;

public class Rat : Enemy
{
    public Rat(int health = 30, int armor = 5, int damage = 15)
    {
        _health = health;
        _armor = armor;
        _damage = damage;
    }
    public override char Visual => '~';
    public override string Name => "Rat";

    public override string Description =>
        "Small creature, but one bite might get you infected.";
}