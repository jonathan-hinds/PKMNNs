using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Definition")]
public class PokemonDefinition : ScriptableObject
{
    public string identifier;
    public string displayName;
    [TextArea]
    public string description;
    public PokemonBaseStats baseStats;

    public string Identifier => identifier;
}
