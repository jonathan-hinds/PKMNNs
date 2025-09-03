using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Stat Stage")]
public class StatStageEffect : BattleEffect
{
    public Stat stat;
    public int stages;
    public bool targetSelf;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        var p = targetSelf ? user : target;
        p?.SetStatStage(stat, stages);
    }
}
