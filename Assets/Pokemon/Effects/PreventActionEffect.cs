using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Prevent Action")]
    public class PreventActionEffect : BattleEffect
    {
        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            if (context != null)
                context.preventMove = true;
        }
    }
}
