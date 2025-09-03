using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class PokemonDefinition
{
    [SerializeField]
    private string id;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;
    [SerializeField]
    private PokemonType[] types;
    [SerializeField]
    private PokemonBaseStats baseStats;
    [SerializeField]
    private GrowthRate growthRate;
    [SerializeField]
    private int catchRate;
    [SerializeField]
    private int expYield;
    [SerializeField]
    private List<string> abilities;
    [SerializeField]
    private List<LearnsetEntry> learnset;
    [SerializeField]
    private List<Evolution> evolutions;

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

[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "PKMN/Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    [SerializeField]
    private List<PokemonDefinition> pokemon = new();

    public PokemonDefinition GetById(string id)
    {
        return pokemon.Find(p => p.Id == id);
    }
}
