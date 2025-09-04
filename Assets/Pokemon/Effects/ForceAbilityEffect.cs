using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Force Ability")]
    public class ForceAbilityEffect : BattleEffect
    {
        public string abilityId;

        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            target?.SetAbility(abilityId);
        }
    }
}
