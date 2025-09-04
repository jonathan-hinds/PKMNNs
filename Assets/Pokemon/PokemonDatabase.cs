using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "PKMN/Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    public List<PokemonDefinition> pokemon = new();

    public PokemonDefinition GetById(string id)
    {
        return pokemon.Find(p => p != null && p.Id == id);
    }

    public IReadOnlyList<PokemonDefinition> All => pokemon;
}

