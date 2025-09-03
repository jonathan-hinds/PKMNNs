using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonDefinition
{
    public string id;
    public string displayName;
    [TextArea]
    public string description;
    public PokemonBaseStats baseStats;
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
