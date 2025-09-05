using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a collection of Pokémon for a trainer or the player.
/// The first slot represents the active Pokémon when entering battle.
/// </summary>
public class PokemonParty : MonoBehaviour
{
    [Serializable]
    public class Slot
    {
        [PokemonId] public string pokemonId;
        [Range(1,100)] public int level = 5;
        public string heldItem;
        [SerializeField] public List<string> moves = new();
    }

    [SerializeField] private PokemonDatabase pokemonDatabase;
    [SerializeField] private List<Slot> startingMembers = new();

    private readonly List<PokemonInstance> members = new();
    public IReadOnlyList<PokemonInstance> Members => members;
    public PokemonDatabase Database => pokemonDatabase;

    public PokemonInstance ActivePokemon => members.Count > 0 ? members[0] : null;

    public int SelectedIndex { get; private set; }
    public PokemonInstance Selected => (SelectedIndex >= 0 && SelectedIndex < members.Count) ? members[SelectedIndex] : null;

    public event Action OnPartyUpdated;
    public event Action<int> OnSelectionChanged;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Builds the runtime party list from the inspector configuration.
    /// </summary>
    public void Initialize()
    {
        members.Clear();
        if (pokemonDatabase == null)
            return;

        foreach (var slot in startingMembers)
        {
            if (string.IsNullOrWhiteSpace(slot.pokemonId))
                continue;
            var def = pokemonDatabase.GetById(slot.pokemonId);
            if (def == null)
                continue;
            var inst = new PokemonInstance(def, slot.level, slot.moves);
            inst.SetHeldItem(slot.heldItem);
            members.Add(inst);
        }
        if (members.Count == 0)
            SelectedIndex = -1;
        else
            SelectedIndex = Mathf.Clamp(SelectedIndex, 0, members.Count - 1);
        OnPartyUpdated?.Invoke();
        OnSelectionChanged?.Invoke(SelectedIndex);
    }

    /// <summary>
    /// Adds a Pokémon to the party if space is available.
    /// </summary>
    public bool AddPokemon(string pokemonId, int level, IEnumerable<string> moves = null, string heldItem = null)
    {
        if (pokemonDatabase == null || members.Count >= 6)
            return false;
        var def = pokemonDatabase.GetById(pokemonId);
        if (def == null)
            return false;
        var inst = new PokemonInstance(def, level, moves);
        inst.SetHeldItem(heldItem);
        members.Add(inst);
        OnPartyUpdated?.Invoke();
        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= members.Count)
            return;
        int previousSelection = SelectedIndex;
        var previousSelectedPokemon = Selected;
        members.RemoveAt(index);
        if (index < SelectedIndex)
            SelectedIndex--;
        else if (SelectedIndex >= members.Count)
            SelectedIndex = members.Count - 1;
        OnPartyUpdated?.Invoke();
        if (previousSelection != SelectedIndex || previousSelectedPokemon != Selected)
            OnSelectionChanged?.Invoke(SelectedIndex);
    }

    public void Swap(int a, int b)
    {
        if (a < 0 || b < 0 || a >= members.Count || b >= members.Count || a == b)
            return;
        (members[a], members[b]) = (members[b], members[a]);
        int previousSelection = SelectedIndex;
        if (SelectedIndex == a)
            SelectedIndex = b;
        else if (SelectedIndex == b)
            SelectedIndex = a;
        OnPartyUpdated?.Invoke();
        if (previousSelection != SelectedIndex)
            OnSelectionChanged?.Invoke(SelectedIndex);
    }

    public void SetSelectedIndex(int index)
    {
        if (index < 0 || index >= members.Count)
            return;
        SelectedIndex = index;
        OnSelectionChanged?.Invoke(SelectedIndex);
    }

    public void NextSelection() => SetSelectedIndex(Mathf.Min(SelectedIndex + 1, members.Count - 1));
    public void PreviousSelection() => SetSelectedIndex(Mathf.Max(SelectedIndex - 1, 0));
}
