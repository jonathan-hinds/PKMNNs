using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Heal")]
public class HealEffect : BattleEffect
{
    [Range(0f,1f)] public float fraction = 0.5f;
    public bool targetSelf = true;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        var p = targetSelf ? user : target;
        if (p != null)
        {
            int amount = Mathf.RoundToInt(p.MaxHP * fraction);
            p.ModifyHP(amount);
        }
    }
}
