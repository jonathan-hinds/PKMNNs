using System;
using System.Collections.Generic;
using UnityEngine;

public class BattlePokemon
{
    public PokemonInstance instance;

    private string currentAbility;
    private string originalAbility;

    private readonly Dictionary<Stat, int> statStages = new();
    private readonly List<StatusInstance> statuses = new();

    private readonly System.Random rng = new();

    private MoveDefinition chargingMove;
    private float queuedRecoil;

    private string rampageMoveId;
    private int rampageTurnsRemaining;
    private string rampagePostStatus;

    private readonly StatusDatabase statusDb;
    private float speedMultiplier = 1f;

    public int MaxHP { get; }
    public int CurrentHP { get; private set; }

    public BattlePokemon(PokemonInstance instance, StatusDatabase statusDb = null)
    {
        this.instance = instance;
        this.statusDb = statusDb;
        currentAbility = instance.Abilities.Count > 0 ? instance.Abilities[0] : null;
        originalAbility = currentAbility;
        MaxHP = instance.Stats.hp;
        CurrentHP = MaxHP;
    }

    public string CurrentAbility => currentAbility;
    public IReadOnlyList<StatusInstance> Statuses => statuses;
    public float SpeedMultiplier => speedMultiplier;

    public void SetAbility(string abilityId) => currentAbility = abilityId;
    public void RestoreOriginalAbility() => currentAbility = originalAbility;

    public void ModifyHP(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);
    }

    public void SetStatStage(Stat stat, int delta)
    {
        int current = statStages.ContainsKey(stat) ? statStages[stat] : 0;
        current = Mathf.Clamp(current + delta, -6, 6);
        statStages[stat] = current;
    }

    public int GetStatStage(Stat stat) => statStages.TryGetValue(stat, out var v) ? v : 0;

    public void ApplyStatus(string id, int duration, BattlePokemon source = null)
    {
        int dur = duration;
        if (dur <= 0 && statusDb != null)
        {
            var def = statusDb.GetById(id);
            if (def != null)
                dur = rng.Next(def.minDuration, def.maxDuration + 1);
        }
        statuses.Add(new StatusInstance { id = id, remainingTurns = dur, data = source });
    }

    public void ClearStatus(string id)
    {
        statuses.RemoveAll(s => s.id == id);
    }

    private void RunStatusHooks(BattleEvent evt, BattlePokemon opponent, MoveDefinition move, BattleContext context)
    {
        if (statusDb == null)
            return;
        foreach (var s in statuses)
        {
            var def = statusDb.GetById(s.id);
            if (def == null || def.hooks == null)
                continue;
            foreach (var hook in def.hooks)
            {
                if (hook.trigger != evt)
                    continue;
                BattlePokemon user = this;
                BattlePokemon target = opponent;
                if (s.data is BattlePokemon source && evt == BattleEvent.TurnEnd)
                {
                    user = source;
                    target = this;
                }
                hook.effect?.Apply(user, target, move, context);
            }
        }
    }

    public bool BeforeMove(BattlePokemon opponent, MoveDefinition move, BattleContext context)
    {
        RunStatusHooks(BattleEvent.BeforeMove, opponent, move, context);
        return context == null || !context.preventMove;
    }

    public void StartCharge(MoveDefinition move)
    {
        chargingMove = move;
    }

    public bool IsCharging => chargingMove != null;

    public MoveDefinition FinishCharge()
    {
        var m = chargingMove;
        chargingMove = null;
        return m;
    }

    public void QueueRecoil(float fraction)
    {
        queuedRecoil = Mathf.Max(queuedRecoil, fraction);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void ApplyQueuedRecoil()
    {
        if (queuedRecoil > 0f)
        {
            int dmg = Mathf.RoundToInt(MaxHP * queuedRecoil);
            ModifyHP(-dmg);
            queuedRecoil = 0f;
        }
    }

    public void StartRampage(string moveId, int minTurns, int maxTurns, string postStatus)
    {
        rampageMoveId = moveId;
        rampageTurnsRemaining = rng.Next(minTurns, maxTurns + 1);
        rampagePostStatus = postStatus;
    }

    public string GetForcedMove() => rampageMoveId;

    public void AdvanceTurn(BattlePokemon opponent)
    {
        RunStatusHooks(BattleEvent.TurnEnd, opponent, null, new BattleContext());

        if (rampageMoveId != null)
        {
            rampageTurnsRemaining--;
            if (rampageTurnsRemaining <= 0)
            {
                rampageMoveId = null;
                if (!string.IsNullOrEmpty(rampagePostStatus))
                    ApplyStatus(rampagePostStatus, 0);
                rampagePostStatus = null;
            }
        }

        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            var s = statuses[i];
            if (s.remainingTurns > 0)
            {
                s.remainingTurns--;
                if (s.remainingTurns <= 0)
                    statuses.RemoveAt(i);
            }
        }

        ApplyQueuedRecoil();
    }
}
