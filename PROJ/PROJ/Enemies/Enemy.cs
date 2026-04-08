using System.ComponentModel.DataAnnotations;
using PROJ.GameConstansts;

namespace PROJ.Enemies;

public abstract class Enemy : BoardObject
{
    protected int _armor;
    protected int _health;
    protected int _damage;
    public override bool Blocker => true;
    public override bool Pickupable => false;
    public override void PickUp(Player player) // to do zmiany TODO
    {
        
    }

    public Enemy(int armor = GameConstants.BaseEnemyArmor, int health = GameConstants.BaseEnemyHealth, int damage = GameConstants.BaseEnemyDamage)
    {
        _armor = armor;
        _health = health;
        _damage = damage;
    }

    public virtual int Armor => _armor;

    public virtual int Health => _health;
    public virtual int Damage => _damage;
    public override bool Fightable => true;

    public virtual void Hit(int dmg)
    {
        if (_armor > 0)
        {
            _armor = int.Max(0, _armor - dmg);
        }
        else
        {
            _health = int.Max(0, _health - dmg);
        }
    }

    public virtual bool Dead()
    {
        return _health == 0;
    }

    public void Attack(Player p, int defense)
    {
        int attackValue = Int32.Max(Damage - defense,0);
        
        p.Health = int.Max(p.Health - attackValue,0);
    }
}