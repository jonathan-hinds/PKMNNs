using UnityEngine;

public static class EffectResolver
{
    [System.Serializable]
    private class SetAbilityArgs
    {
        public string abilityId;
    }

    [System.Serializable]
    private class RampageArgs
    {
        public int minTurns;
        public int maxTurns;
        public string postStatus;
    }

    public static void ApplyEffect(EffectCatalog catalog, BattlePokemon user, BattlePokemon target, MoveDefinition move, EffectData data)
    {
        // Resolve arguments from the catalog entry unless the move overrides them
        var entry = catalog != null ? catalog.GetById(data.effectId) : null;
        var argsJson = string.IsNullOrEmpty(data.overrideArgsJson) ? entry?.argsJson : data.overrideArgsJson;

        switch (data.effectId)
        {
            case "SetAbility":
                var sa = JsonUtility.FromJson<SetAbilityArgs>(argsJson);
                if (sa != null)
                    target.SetAbility(sa.abilityId);
                break;
            case "Rampage":
                var ra = JsonUtility.FromJson<RampageArgs>(argsJson);
                if (ra != null)
                    user.StartRampage(move.id, ra.minTurns, ra.maxTurns, ra.postStatus);
                break;
        }
    }
}
