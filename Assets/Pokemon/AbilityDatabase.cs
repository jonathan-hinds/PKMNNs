using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "PKMN/Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    public List<AbilityDefinition> abilities = new();

    public AbilityDefinition GetById(string id)
    {
        return abilities.Find(a => a.id == id);
    }
}
