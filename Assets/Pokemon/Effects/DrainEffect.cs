using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Drain")]
public class DrainEffect : BattleEffect
{
    [Range(0f,1f)] public float fraction = 0.125f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        if (user == null || target == null)
            return;
        int dmg = Mathf.RoundToInt(target.MaxHP * fraction);
        target.ModifyHP(-dmg);
        user.ModifyHP(dmg);
    }
}
