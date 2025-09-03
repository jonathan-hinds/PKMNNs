using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Database")]
public class PokemonDatabase : ScriptableObject
{
    [SerializeField]
    private List<PokemonDefinition> definitions = new();

    private Dictionary<string, PokemonDefinition> lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, PokemonDefinition>();
        foreach (var def in definitions)
        {
            if (def == null || string.IsNullOrEmpty(def.identifier))
                continue;
            lookup[def.identifier] = def;
        }
    }

    public PokemonDefinition GetById(string id)
    {
        if (lookup == null || lookup.Count != definitions.Count)
            BuildLookup();
        lookup.TryGetValue(id, out var def);
        return def;
    }
}
