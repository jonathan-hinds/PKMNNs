using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonInstance
{
    private readonly PokemonDefinition definition;
    private readonly int level;

    private readonly List<string> moveSet;
    private readonly List<string> learnableMoves;

    public PokemonDefinition Definition => definition;
    public int Level => level;

    public PokemonStats Stats => PokemonStatCalculator.Calculate(definition.BaseStats, level);

    public IReadOnlyList<string> Moves => moveSet;
    public IReadOnlyList<string> LearnableMoves => learnableMoves;
    public IReadOnlyList<string> Abilities => definition.Abilities;
    public IReadOnlyList<PokemonType> Types => definition.Types;

    public PokemonInstance(PokemonDefinition definition, int level)
    {
        this.definition = definition;
        this.level = Mathf.Clamp(level, 1, 100);

        learnableMoves = GetMovesForLevel(this.level);
        moveSet = GenerateMoveLoadout(learnableMoves);
    }

    private List<string> GetMovesForLevel(int targetLevel)
    {
        var learned = new List<string>();
        var ls = definition.Learnset;
        if (ls == null || ls.Count == 0)
            return learned;

        foreach (var entry in ls)
        {
            if (entry == null) continue;
            if (entry.level <= targetLevel && entry.moves != null)
            {
                foreach (var move in entry.moves)
                {
                    if (string.IsNullOrWhiteSpace(move) || learned.Contains(move))
                        continue;
                    learned.Add(move);
                }
            }
        }
        return learned;
    }

    private List<string> GenerateMoveLoadout(List<string> available)
    {
        if (available == null || available.Count == 0)
            return new List<string>();

        // Take the last four moves learned to mirror main-series behavior
        int count = Mathf.Min(4, available.Count);
        return available.Skip(available.Count - count).Take(count).ToList();
    }
}
