using UnityEngine;

public class PokemonInstance
{
    public PokemonDefinition Definition { get; private set; }
    public int Level { get; private set; }

    public int MaxHP { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int SpecialAttack { get; private set; }
    public int SpecialDefense { get; private set; }
    public int Speed { get; private set; }

    private PokemonInstance() { }

    public static PokemonInstance Create(PokemonDefinition definition, int level)
    {
        var instance = new PokemonInstance
        {
            Definition = definition,
            Level = Mathf.Max(1, level)
        };
        instance.RecalculateStats();
        return instance;
    }

    public void RecalculateStats()
    {
        int l = Mathf.Max(1, Level);
        var stats = Definition.baseStats;
        MaxHP = CalculateHp(stats.hp, l);
        Attack = CalculateStat(stats.attack, l);
        Defense = CalculateStat(stats.defense, l);
        SpecialAttack = CalculateStat(stats.specialAttack, l);
        SpecialDefense = CalculateStat(stats.specialDefense, l);
        Speed = CalculateStat(stats.speed, l);
    }

    private static int CalculateHp(int baseStat, int level)
    {
        return Mathf.FloorToInt(((2 * baseStat) * level) / 100f) + level + 10;
    }

    private static int CalculateStat(int baseStat, int level)
    {
        return Mathf.FloorToInt(((2 * baseStat) * level) / 100f) + 5;
    }
}
