using System.Collections.Generic;
using System.Linq;

public class PokemonInstance
{
    public PokemonDefinition definition;
    public int level;

    private List<string> moveSet;
    private List<string> learnableMoves;

    private static System.Random rng = new System.Random();

    public PokemonStats Stats => PokemonStatCalculator.Calculate(definition.baseStats, level);
    public IReadOnlyList<string> Moves => moveSet;
    public IReadOnlyList<string> LearnableMoves => learnableMoves;
    public IReadOnlyList<string> Abilities => definition.abilities;
    public IReadOnlyList<PokemonType> Types => definition.types;

    public PokemonInstance(PokemonDefinition definition, int level)
    {
        this.definition = definition;
        this.level = level;

        learnableMoves = GetMovesForLevel(level);
        moveSet = GenerateRandomMoveLoadout(learnableMoves);
    }

    private List<string> GetMovesForLevel(int targetLevel)
    {
        var learned = new HashSet<string>();
        if (definition.learnset == null)
            return learned.ToList();
        foreach (var entry in definition.learnset)
        {
            if (entry.level <= targetLevel)
            {
                foreach (var move in entry.moves)
                    learned.Add(move);
            }
        }
        return learned.ToList();
    }

    private List<string> GenerateRandomMoveLoadout(List<string> available)
    {
        var result = new List<string>(available);
        if (result.Count <= 4)
            return result;

        // Fisher-Yates shuffle then take first four
        int n = result.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (result[n], result[k]) = (result[k], result[n]);
        }
        return result.GetRange(0, 4);
    }
}
