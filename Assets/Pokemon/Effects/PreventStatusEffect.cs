using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/PreventStatus")]
    public class PreventStatusEffect : BattleEffect
    {
        public string statusId;

        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            if (context != null && context.statusId == statusId)
            {
                context.preventStatus = true;
            }
        }
    }
}
