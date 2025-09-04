using System.Collections.Generic;
using UnityEngine;
using PKMN;

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

[CreateAssetMenu(fileName = "MoveDatabase", menuName = "PKMN/Move Database")]
public class MoveDatabase : ScriptableObject
{
    public List<MoveDefinition> moves = new();

    public MoveDefinition GetById(string id)
    {
        return moves.Find(m => m.id == id);
    }
}
