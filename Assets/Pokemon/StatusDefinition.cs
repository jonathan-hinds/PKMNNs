using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusDefinition
{
    public string id;
    public int minDuration;
    public int maxDuration;
    public List<EffectHook> hooks;
}

[CreateAssetMenu(fileName = "StatusDatabase", menuName = "PKMN/Status Database")]
public class StatusDatabase : ScriptableObject
{
    public List<StatusDefinition> statuses;

    public StatusDefinition GetById(string id)
    {
        return statuses.Find(s => s.id == id);
    }
}
