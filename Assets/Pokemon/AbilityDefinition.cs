using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityDefinition
{
    public string id;
    public string name;
    [TextArea]
    public string summary;
    public List<EffectHook> hooks = new();
}

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "PKMN/Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    public List<AbilityDefinition> abilities = new();

    public AbilityDefinition GetById(string id)
    {
        return abilities.Find(a => a.id == id);
    }
}
