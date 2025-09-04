using UnityEngine;

namespace PKMN
{
    public class BattleContext
    {
        // Flag that prevents a Pokemon from acting this turn
        public bool preventMove;
        // Multiplier applied to move power during damage calculation
        public float powerMultiplier = 1f;
        // Id of a status currently being attempted
        public string statusId;
        // Flag that blocks the status from being applied
        public bool preventStatus;
    }

    public abstract class BattleEffect : ScriptableObject
    {
        public virtual void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
        }
    }
}
