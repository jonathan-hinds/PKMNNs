using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Damage")]
    public class DamageEffect : BattleEffect
    {
        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            if (target != null)
                target.ModifyHP(-move.power);
        }
    }
}
