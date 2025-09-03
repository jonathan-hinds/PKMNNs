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

    public static void ApplyEffect(BattlePokemon user, BattlePokemon target, MoveDefinition move, EffectData data)
    {
        switch (data.effect)
        {
            case "SetAbility":
                var sa = JsonUtility.FromJson<SetAbilityArgs>(data.argsJson);
                if (sa != null)
                    target.SetAbility(sa.abilityId);
                break;
            case "Rampage":
                var ra = JsonUtility.FromJson<RampageArgs>(data.argsJson);
                if (ra != null)
                    user.StartRampage(move.id, ra.minTurns, ra.maxTurns, ra.postStatus);
                break;
        }
    }
}
