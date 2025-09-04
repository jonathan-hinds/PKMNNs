using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveDatabase", menuName = "PKMN/Move Database")]
public class MoveDatabase : ScriptableObject
{
    public List<MoveDefinition> moves = new();

    public MoveDefinition GetById(string id)
    {
        return moves.Find(m => m.id == id);
    }
}
