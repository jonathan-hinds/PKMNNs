#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Utility helpers for loading or creating database assets.
/// </summary>
static class DatabaseLoader
{
    public static T LoadOrCreate<T>(string path) where T : ScriptableObject
    {
        var asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
        }
        return asset;
    }
}

/// <summary>
/// Wizard for creating a new move entry.
/// </summary>
public class MoveCreatorWizard : ScriptableWizard
{
    public string id;
    public string name;
    public PokemonType type;
    public MoveCategory category;
    public int power;
    public int accuracy = 100;
    public int pp = 10;
    public BattleEffect[] effects;

    [MenuItem("PKMN/Create/Move")]
    static void CreateWizard() => DisplayWizard<MoveCreatorWizard>("Create Move", "Create");

    void OnWizardCreate()
    {
        var db = DatabaseLoader.LoadOrCreate<MoveDatabase>("Assets/Pokemon/MoveDatabase.asset");
        var def = new MoveDefinition
        {
            id = id,
            name = name,
            type = type,
            category = category,
            power = power,
            accuracy = accuracy,
            pp = pp,
            effects = new List<BattleEffect>(effects ?? new BattleEffect[0])
        };
        db.moves.Add(def);
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
    }
}

/// <summary>
/// Wizard for creating a new ability entry.
/// </summary>
public class AbilityCreatorWizard : ScriptableWizard
{
    public string id;
    public string name;
    [TextArea]
    public string summary;
    public EffectHook[] hooks;

    [MenuItem("PKMN/Create/Ability")]
    static void CreateWizard() => DisplayWizard<AbilityCreatorWizard>("Create Ability", "Create");

    void OnWizardCreate()
    {
        var db = DatabaseLoader.LoadOrCreate<AbilityDatabase>("Assets/Pokemon/AbilityDatabase.asset");
        var def = new AbilityDefinition
        {
            id = id,
            name = name,
            summary = summary,
            hooks = new List<EffectHook>(hooks ?? new EffectHook[0])
        };
        db.abilities.Add(def);
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
    }
}

/// <summary>
/// Wizard for creating a new status entry.
/// </summary>
public class StatusCreatorWizard : ScriptableWizard
{
    public string id;
    public int minDuration;
    public int maxDuration;
    public EffectHook[] hooks;

    [MenuItem("PKMN/Create/Status")]
    static void CreateWizard() => DisplayWizard<StatusCreatorWizard>("Create Status", "Create");

    void OnWizardCreate()
    {
        var db = DatabaseLoader.LoadOrCreate<StatusDatabase>("Assets/Pokemon/StatusDatabase.asset");
        var def = new StatusDefinition
        {
            id = id,
            minDuration = minDuration,
            maxDuration = maxDuration,
            hooks = new List<EffectHook>(hooks ?? new EffectHook[0])
        };
        db.statuses.Add(def);
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
    }
}

/// <summary>
/// Wizard for creating a new Pokémon entry.
/// </summary>
public class PokemonCreatorWizard : ScriptableWizard
{
    public string id;
    public string displayName;
    [TextArea]
    public string description;
    public PokemonType[] types = new PokemonType[2];
    public PokemonBaseStats baseStats;
    public GrowthRate growthRate;
    public int catchRate;
    public int expYield;
    public string[] abilities;
    public LearnsetEntry[] learnset;
    public Evolution[] evolutions;

    [MenuItem("PKMN/Create/Pokemon")]
    static void CreateWizard() => DisplayWizard<PokemonCreatorWizard>("Create Pokemon", "Create");

    void OnWizardCreate()
    {
        var db = DatabaseLoader.LoadOrCreate<PokemonDatabase>("Assets/Pokemon/PokemonDatabase.asset");
        var def = new PokemonDefinition
        {
            id = id,
            displayName = displayName,
            description = description,
            types = types,
            baseStats = baseStats,
            growthRate = growthRate,
            catchRate = catchRate,
            expYield = expYield,
            abilities = new List<string>(abilities ?? new string[0]),
            learnset = new List<LearnsetEntry>(learnset ?? new LearnsetEntry[0]),
            evolutions = new List<Evolution>(evolutions ?? new Evolution[0])
        };
        db.pokemon.Add(def);
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
    }
}

#endif
