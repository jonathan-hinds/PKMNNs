public class PokemonInstance
{
    public PokemonDefinition definition;
    public int level;

    public PokemonStats Stats => PokemonStatCalculator.Calculate(definition.baseStats, level);

    public PokemonInstance(PokemonDefinition definition, int level)
    {
        this.definition = definition;
        this.level = level;
    }
}
