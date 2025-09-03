using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Charge")]
public class ChargeEffect : BattleEffect
{
    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.StartCharge(move);
    }
}
