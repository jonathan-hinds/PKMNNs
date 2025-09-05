#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PokemonIdAttribute))]
public class PokemonIdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var db = AssetDatabase.LoadAssetAtPath<PokemonDatabase>("Assets/Pokemon/PokemonDatabase.asset");
        if (db == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        var ids = db.pokemon.Where(p => p != null).Select(p => p.Id).ToList();
        if (ids.Count == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        int index = Mathf.Max(0, ids.IndexOf(property.stringValue));
        index = EditorGUI.Popup(position, label.text, index, ids.ToArray());
        property.stringValue = ids[index];
    }
}
#endif
