using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Self Hit Chance")]
public class SelfHitChanceEffect : BattleEffect
{
    [Range(0f,1f)] public float chance = 0.33f;
    public int power = 40;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        if (user == null || context == null)
            return;
        if (UnityEngine.Random.value < chance)
        {
            user.ModifyHP(-power);
            context.preventMove = true;
        }
    }
}
