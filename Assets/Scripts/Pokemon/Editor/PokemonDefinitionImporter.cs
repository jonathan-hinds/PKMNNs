#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class PokemonDefinitionImporter
{
    [MenuItem("Pokemon/Import Definitions From CSV")]
    public static void ImportFromCsv()
    {
        string path = EditorUtility.OpenFilePanel("Pokemon CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path))
            return;

        string folder = "Assets/PokemonData/Definitions";
        if (!AssetDatabase.IsValidFolder("Assets/PokemonData"))
            AssetDatabase.CreateFolder("Assets", "PokemonData");
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/PokemonData", "Definitions");

        var lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var cols = line.Split(',');
            if (cols.Length < 9)
                continue;

            var def = ScriptableObject.CreateInstance<PokemonDefinition>();
            def.identifier = cols[0].Trim();
            def.displayName = cols[1].Trim();
            def.description = cols[2].Trim();
            def.baseStats = new PokemonBaseStats
            {
                hp = int.Parse(cols[3]),
                attack = int.Parse(cols[4]),
                defense = int.Parse(cols[5]),
                specialAttack = int.Parse(cols[6]),
                specialDefense = int.Parse(cols[7]),
                speed = int.Parse(cols[8])
            };

            AssetDatabase.CreateAsset(def, $"{folder}/{def.identifier}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif
