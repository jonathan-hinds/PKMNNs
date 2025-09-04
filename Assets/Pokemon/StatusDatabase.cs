using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusDatabase", menuName = "PKMN/Status Database")]
public class StatusDatabase : ScriptableObject
{
    public List<StatusDefinition> statuses = new();

    public StatusDefinition GetById(string id)
    {
        return statuses.Find(s => s.id == id);
    }
}
