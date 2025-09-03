using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Set Speed Multiplier")]
public class SetSpeedMultiplierEffect : BattleEffect
{
    public float multiplier = 2f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.SetSpeedMultiplier(multiplier);
    }
}
