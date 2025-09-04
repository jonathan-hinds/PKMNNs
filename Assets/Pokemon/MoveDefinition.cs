using System.Collections.Generic;
using UnityEngine;

namespace PKMN
{
    [System.Serializable]
    public class MoveDefinition
    {
        public string id;
        public string name;
        public PokemonType type;
        public MoveCategory category;
        public int power;
        public int accuracy;
        public int pp;
        public List<BattleEffect> effects = new();
    }
}
