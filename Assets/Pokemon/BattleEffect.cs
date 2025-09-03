using UnityEngine;

public class BattleContext
{
    // Flag that prevents a Pokemon from acting this turn
    public bool preventMove;
    // Multiplier applied to move power during damage calculation
    public float powerMultiplier = 1f;
}

public abstract class BattleEffect : ScriptableObject
{
    public virtual void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
    }
}

[CreateAssetMenu(menuName="PKMN/Effects/Damage")]
public class DamageEffect : BattleEffect
{
    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        if (target != null)
            target.ModifyHP(-move.power);
    }
}

public enum Stat
{
    Attack,
    Defense,
    SpecialAttack,
    SpecialDefense,
    Speed,
    Accuracy,
    Evasion
}

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

[CreateAssetMenu(menuName="PKMN/Effects/Recoil")]
public class RecoilEffect : BattleEffect
{
    [Range(0f,1f)] public float fraction = 0.25f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.QueueRecoil(fraction);
    }
}

[CreateAssetMenu(menuName="PKMN/Effects/Force Ability")]
public class ForceAbilityEffect : BattleEffect
{
    public string abilityId;
    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        target?.SetAbility(abilityId);
    }
}

[CreateAssetMenu(menuName="PKMN/Effects/Charge")]
public class ChargeEffect : BattleEffect
{
    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.StartCharge(move);
    }
}

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

[CreateAssetMenu(menuName="PKMN/Effects/Prevent Action")]
public class PreventActionEffect : BattleEffect
{
    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        if (context != null)
            context.preventMove = true;
    }
}

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

[CreateAssetMenu(menuName="PKMN/Effects/Power Multiplier")]
public class PowerMultiplierEffect : BattleEffect
{
    public PokemonType type;
    [Range(0f,1f)] public float hpThreshold = 1f;
    public float multiplier = 1f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        if (user == null || move == null || context == null)
            return;
        if (move.type == type && user.CurrentHP <= user.MaxHP * hpThreshold)
            context.powerMultiplier *= multiplier;
    }
}

[CreateAssetMenu(menuName="PKMN/Effects/Set Speed Multiplier")]
public class SetSpeedMultiplierEffect : BattleEffect
{
    public float multiplier = 2f;

    public override void Apply(BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
    {
        user?.SetSpeedMultiplier(multiplier);
    }
}
