using UnityEngine;

public class BattleContext
{
    // Flag that prevents a Pokemon from acting this turn
    public bool preventMove;
    // Multiplier applied to move power during damage calculation
    public float powerMultiplier = 1f;
}

public abstract class BattleEffect : ScriptableObject
{
    public virtual void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
    }
}
