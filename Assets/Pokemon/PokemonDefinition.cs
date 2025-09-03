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
    public string id;
    public string displayName;
    [TextArea]
    public string description;
    public PokemonType[] types;
    public PokemonBaseStats baseStats;
    public GrowthRate growthRate;
    public int catchRate;
    public int expYield;
    public List<string> abilities;
    public List<LearnsetEntry> learnset;
    public List<Evolution> evolutions;
}

[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "PKMN/Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    public List<PokemonDefinition> pokemon;

    public PokemonDefinition GetById(string id)
    {
        return pokemon.Find(p => p.id == id);
    }
}
