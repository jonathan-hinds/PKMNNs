using System;
using System.Collections.Generic;

public class BattlePokemon
{
    public PokemonInstance instance;

    private string currentAbility;
    private string originalAbility;

    private List<string> statuses = new List<string>();

    private string rampageMoveId;
    private int rampageTurnsRemaining;
    private string rampagePostStatus;

    private static Random rng = new Random();

    public BattlePokemon(PokemonInstance instance)
    {
        this.instance = instance;
        currentAbility = instance.Abilities.Count > 0 ? instance.Abilities[0] : null;
        originalAbility = currentAbility;
    }

    public string CurrentAbility => currentAbility;
    public IReadOnlyList<string> Statuses => statuses;

    public void SetAbility(string abilityId)
    {
        currentAbility = abilityId;
    }

    public void RestoreOriginalAbility()
    {
        currentAbility = originalAbility;
    }

    public void OnSwitchOut()
    {
        rampageMoveId = null;
        rampageTurnsRemaining = 0;
        rampagePostStatus = null;
        RestoreOriginalAbility();
    }

    public void StartRampage(string moveId, int minTurns, int maxTurns, string postStatus)
    {
        rampageMoveId = moveId;
        rampageTurnsRemaining = rng.Next(minTurns, maxTurns + 1);
        rampagePostStatus = postStatus;
    }

    public string GetForcedMove()
    {
        return rampageMoveId;
    }

    public void AdvanceTurn()
    {
        if (rampageMoveId != null)
        {
            rampageTurnsRemaining--;
            if (rampageTurnsRemaining <= 0)
            {
                rampageMoveId = null;
                if (!string.IsNullOrEmpty(rampagePostStatus))
                    statuses.Add(rampagePostStatus);
                rampagePostStatus = null;
            }
        }
    }
}
