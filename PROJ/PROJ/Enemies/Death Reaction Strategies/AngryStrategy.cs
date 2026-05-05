using PROJ.Enemies.Species.Interfaces;

namespace PROJ.Enemies.Death_Reaction_Strategies;

public class AngryStrategy:  IDeathReactionStrategy
{
    public void ReactToDeath(Enemy e)
    {
        e.ModifyDamage(5);
        e.Blink(ConsoleColor.Red);
    }
}