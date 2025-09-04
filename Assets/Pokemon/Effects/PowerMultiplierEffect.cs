using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Power Multiplier")]
    public class PowerMultiplierEffect : BattleEffect
    {
        public PokemonType type;
        [Range(0f,1f)] public float hpThreshold = 1f;
        public float multiplier = 1f;

        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            if (user == null || move == null || context == null)
                return;
            if (move.type == type && user.CurrentHP <= user.MaxHP * hpThreshold)
                context.powerMultiplier *= multiplier;
        }
    }
}
