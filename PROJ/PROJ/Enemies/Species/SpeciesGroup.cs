using PROJ.Enemies.Species.Interfaces;
using PROJ.Enemies.Death_Reaction_Strategies;

namespace PROJ.Enemies.Species;

public class SpeciesGroup
{
    private string _name;

    private List<ISpeciesObserver> _members = new();
    private IDeathReactionStrategy _strategy;
    public virtual string Name => _name;

    public SpeciesGroup(string name, IDeathReactionStrategy? strategy = null)
    {
        _name = name;
        if (strategy == null)
            _strategy = new DefaultStrategy();
        else
            _strategy = strategy;
    }

    public void Subscribe(ISpeciesObserver observer) => _members.Add(observer);
    public void Unsubscribe(ISpeciesObserver observer) => _members.Remove(observer);

    public void NotifyDeath()
    {
        foreach (var member in _members)
        {
            member.NotifyDeath();
        }
    }
    
    public void ReactionStrategy(Enemy e)
    {
        _strategy.ReactToDeath(e);
    }
}