namespace PROJ.Enemies;

public class Guard : Enemy
{
    
    public Guard(int health = 200, int armor = 30, int damage = 20)
    {
        _health = health;
        _armor = armor;
        _damage = damage;
    }
    public override char Visual => '⛨';
    public override string Name => "Guard";

    public override string Description =>
        "One of the oligarch's fighters whose sole purpose is to make sure you don't get the medicine";
    
}