using System.Collections.Generic;
using UnityEngine;
// If your old assets used public fields like id/displayName/baseStats, uncomment:
// using UnityEngine.Serialization;

#region Data Rows

[System.Serializable]
public class LearnsetEntry
{
    public int level;
    public List<string> moves;
}

[System.Serializable]
public class Evolution
{
    public string trigger;
    public int minLevel;
    public string to;
}

#endregion

#region Definitions

[System.Serializable]
public class PokemonDefinition
{
    // If migrating from public fields, add FormerlySerializedAs to preserve data:
    // [FormerlySerializedAs("id")]
    [SerializeField] private string id;

    // [FormerlySerializedAs("displayName")]
    [SerializeField] private string displayName;

    [SerializeField, TextArea] private string description;

    [SerializeField] private PokemonType[] types;

    // [FormerlySerializedAs("baseStats")]
    [SerializeField] private PokemonBaseStats baseStats;

    [SerializeField] private GrowthRate growthRate;
    [SerializeField] private int catchRate;
    [SerializeField] private int expYield;

    [SerializeField] private List<string> abilities = new();
    [SerializeField] private List<LearnsetEntry> learnset = new();
    [SerializeField] private List<Evolution> evolutions = new();

    public string Id => id;
    public string DisplayName => displayName;
    public string Description => description;
    public IReadOnlyList<PokemonType> Types => types;
    public PokemonBaseStats BaseStats => baseStats;
    public GrowthRate GrowthRate => growthRate;
    public int CatchRate => catchRate;
    public int ExpYield => expYield;
    public IReadOnlyList<string> Abilities => abilities;
    public IReadOnlyList<LearnsetEntry> Learnset => learnset;
    public IReadOnlyList<Evolution> Evolutions => evolutions;
}

#endregion

#region Database

[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "PKMN/Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    [SerializeField] private List<PokemonDefinition> pokemon = new();

    public PokemonDefinition GetById(string id)
    {
        return pokemon.Find(p => p != null && p.Id == id);
    }

    public IReadOnlyList<PokemonDefinition> All => pokemon;
}

#endregion
