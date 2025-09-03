using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] private int maxSize = 6;
    private readonly List<PokemonInstance> members = new();

    public IReadOnlyList<PokemonInstance> Members => members;

    public bool Add(PokemonInstance pokemon)
    {
        if (pokemon == null || members.Count >= maxSize)
            return false;
        members.Add(pokemon);
        return true;
    }

    public bool Remove(PokemonInstance pokemon)
    {
        return members.Remove(pokemon);
    }

    public void Swap(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= members.Count || indexB < 0 || indexB >= members.Count)
            return;
        (members[indexA], members[indexB]) = (members[indexB], members[indexA]);
    }

    public PokemonInstance this[int index] => members[index];
}
