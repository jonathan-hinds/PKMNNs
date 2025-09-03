using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonInstance
{
    private static readonly System.Random rng = new System.Random();

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
        moveSet = GenerateRandomMoveLoadout(learnableMoves);
    }

    private List<string> GetMovesForLevel(int targetLevel)
    {
        var learned = new HashSet<string>();
        var ls = definition.Learnset;
        if (ls == null || ls.Count == 0)
            return new List<string>();

        foreach (var entry in ls)
        {
            if (entry == null) continue;
            if (entry.level <= targetLevel && entry.moves != null)
            {
                foreach (var move in entry.moves)
                {
                    if (!string.IsNullOrWhiteSpace(move))
                        learned.Add(move);
                }
            }
        }
        return learned.ToList();
    }

    private List<string> GenerateRandomMoveLoadout(List<string> available)
    {
        if (available == null || available.Count == 0)
            return new List<string>();

        var result = new List<string>(available);

        // Fisher–Yates shuffle
        int n = result.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (result[n], result[k]) = (result[k], result[n]);
        }

        if (result.Count > 4)
            result.RemoveRange(4, result.Count - 4);

        return result;
    }
}
