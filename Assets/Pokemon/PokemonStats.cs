using UnityEngine;

namespace PKMN
{
    [System.Serializable]
    public struct PokemonBaseStats
    {
        public int hp;
        public int attack;
        public int defense;
        public int specialAttack;
        public int specialDefense;
        public int speed;
    }

    [System.Serializable]
    public struct PokemonStats
    {
        public int hp;
        public int attack;
        public int defense;
        public int specialAttack;
        public int specialDefense;
        public int speed;
    }

    public static class PokemonStatCalculator
    {
        public static PokemonStats Calculate(PokemonBaseStats baseStats, int level)
        {
            PokemonStats result;
            result.hp = CalculateHP(baseStats.hp, level);
            result.attack = CalculateOther(baseStats.attack, level);
            result.defense = CalculateOther(baseStats.defense, level);
            result.specialAttack = CalculateOther(baseStats.specialAttack, level);
            result.specialDefense = CalculateOther(baseStats.specialDefense, level);
            result.speed = CalculateOther(baseStats.speed, level);
            return result;
        }

        private static int CalculateHP(int baseStat, int level)
        {
            return Mathf.FloorToInt(((2 * baseStat) * level) / 100f) + level + 10;
        }

        private static int CalculateOther(int baseStat, int level)
        {
            return Mathf.FloorToInt(((2 * baseStat) * level) / 100f) + 5;
        }
    }
}
