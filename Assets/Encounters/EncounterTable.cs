using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EncounterEntry
{
    public PokemonDefinition pokemon;
    public int minLevel = 1;
    public int maxLevel = 1;
    public float weight = 1f;
}

public class EncounterTable : MonoBehaviour
{
    [Range(0f, 1f)]
    public float encounterChance = 0.1f;
    public List<EncounterEntry> encounters = new();

    /// <summary>
    /// Attempts to trigger a wild Pokémon encounter.
    /// Logs the encountered Pokémon and level if successful.
    /// </summary>
    /// <returns>True if an encounter occurred.</returns>
    public bool TryEncounter()
    {
        if (encounters.Count == 0)
            return false;
        if (Random.value > encounterChance)
            return false;

        float totalWeight = 0f;
        foreach (var entry in encounters)
            totalWeight += Mathf.Max(0f, entry.weight);

        float roll = Random.value * totalWeight;
        foreach (var entry in encounters)
        {
            roll -= Mathf.Max(0f, entry.weight);
            if (roll <= 0f)
            {
                int level = Random.Range(entry.minLevel, entry.maxLevel + 1);
                string name = entry.pokemon != null ? entry.pokemon.DisplayName : "Unknown";
                Debug.Log($"Encountered Pokémon: {name} (Lv {level})");
                return true;
            }
        }
        return false;
    }
}
