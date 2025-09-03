using UnityEngine;

[CreateAssetMenu(menuName="PKMN/Effects/Recoil")]
public class RecoilEffect : BattleEffect
{
    [Range(0f,1f)] public float fraction = 0.25f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.QueueRecoil(fraction);
    }
}
