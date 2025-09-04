using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Status")]
    public class StatusEffect : BattleEffect
    {
        public string statusId;
        public bool targetSelf;

        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            var p = targetSelf ? user : target;
            p?.ApplyStatus(statusId, 0, user);
        }
    }
}
