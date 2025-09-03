using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/HP Percent Damage")]
public class HPPercentDamageEffect : BattleEffect
{
    [Range(0f,1f)] public float fraction = 0.125f;
    public bool targetSelf;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        var p = targetSelf ? user : target;
        if (p != null)
        {
            int dmg = Mathf.RoundToInt(p.MaxHP * fraction);
            p.ModifyHP(-dmg);
        }
    }
}
