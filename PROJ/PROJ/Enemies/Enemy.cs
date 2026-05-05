using System.ComponentModel.DataAnnotations;
using PROJ.Enemies.Interfaces;
using PROJ.Enemies.Species;
using PROJ.Enemies.Species.Interfaces;
using PROJ.GameConstansts;
using PROJ.Logging.Classes;

namespace PROJ.Enemies;

public abstract class Enemy : BoardObject, ISpeciesObserver, ISoundReceiver
{
    protected int _armor;
    protected int _health;
    protected int _damage;

    protected char vis;
    protected string name;
    protected SpeciesGroup? speciesGroup;
    protected Board EnemyBoard;
    public override bool Blocker => true;
    public override bool Pickupable => false;
    public override void PickUp(Player player) // to do zmiany TODO
    {
        
    }

    public Enemy(Board b, int armor = GameConstants.BaseEnemyArmor, int health = GameConstants.BaseEnemyHealth, int damage = GameConstants.BaseEnemyDamage,
        char vis = 'X', string name = "unnamed", SpeciesGroup? speciesGroup = null)
    {
        _armor = armor;
        _health = health;
        _damage = damage;
        this.vis = vis;
        this.name = name;
        if (speciesGroup != null)
        {
            this.speciesGroup = speciesGroup;
            this.speciesGroup.Subscribe(this);
        }

        var messageBus = PickUpSoundBus.GetInstance;
        messageBus.Subscribe(this);

        EnemyBoard = b;
    }

    public override string Name => name;

    public override char Visual => vis;


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

    public void NotifyDeath()
    {
        speciesGroup?.ReactionStrategy(this);
    }

    // public void ModifyHealth(int amount)
    // {
    //     _health += amount;
    //     if (_health <= 0)
    //         Dead();
    // }
    public void ModifyArmor(int amount)
    {
        _armor += amount;
        if (_armor <= 0)
            _armor = 0;
    }
    public void ModifyDamage(int amount)
    {
        _damage += amount;
        if (_damage <= 0)
            _damage = 0;
    }

    public void Blink(ConsoleColor color)
    {
        EnemyBoard.Blink(X, Y, color);
    }

    public void DeleteYourself()
    {
        speciesGroup?.Unsubscribe(this);
        speciesGroup?.NotifyDeath();
        EnemyBoard.DeleteEnemy(this);

        var messageBus = PickUpSoundBus.GetInstance;
        
        messageBus.UnSubscribe(this);
        
    }

    public void RegisterSound(SoundMessage message)
    {
        var logger = Logger.GetInstance;
        logger.Log($"{Name} Heard {message.GetMessage} From ({message.GetX}, {message.GetY}), {message.GetDist} Tiles Away");
    }
    
}