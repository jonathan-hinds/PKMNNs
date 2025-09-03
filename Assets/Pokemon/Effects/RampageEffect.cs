using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(menuName="PKMN/Effects/Rampage")]
    public class RampageEffect : BattleEffect
    {
        public int minTurns = 2;
        public int maxTurns = 3;
        public string postStatus;

        public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            user?.StartRampage(move.id, minTurns, maxTurns, postStatus);
        }
    }
}
