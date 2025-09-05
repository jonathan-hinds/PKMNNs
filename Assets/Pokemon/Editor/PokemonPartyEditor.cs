#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PokemonParty.Slot))]
public class PokemonPartySlotDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Base height plus four lines for moves when a Pokémon is selected
        var idProp = property.FindPropertyRelative("pokemonId");
        bool hasPokemon = !string.IsNullOrEmpty(idProp.stringValue);
        int lines = hasPokemon ? 3 + 4 : 2; // pokemon + level + held item + moves
        return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var idProp = property.FindPropertyRelative("pokemonId");
        var levelProp = property.FindPropertyRelative("level");
        var itemProp = property.FindPropertyRelative("heldItem");
        var movesProp = property.FindPropertyRelative("moves");

        var line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(line, idProp, new GUIContent("Pokémon"));
        line.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(line, levelProp, new GUIContent("Level"));
        line.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(line, itemProp, new GUIContent("Held Item"));

        if (!string.IsNullOrEmpty(idProp.stringValue))
        {
            var party = property.serializedObject.targetObject as PokemonParty;
            PokemonDefinition def = party != null && party.Database != null ? party.Database.GetById(idProp.stringValue) : null;

            List<string> moves = new();
            if (def != null && def.Learnset != null)
            {
                foreach (var entry in def.Learnset)
                {
                    if (entry?.moves == null) continue;
                    foreach (var m in entry.moves)
                        if (!string.IsNullOrWhiteSpace(m) && !moves.Contains(m))
                            moves.Add(m);
                }
            }
            moves.Insert(0, string.Empty);
            movesProp.arraySize = 4;
            for (int i = 0; i < 4; i++)
            {
                line.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var moveProp = movesProp.GetArrayElementAtIndex(i);
                int selected = Mathf.Max(0, moves.IndexOf(moveProp.stringValue));
                selected = EditorGUI.Popup(line, $"Move {i + 1}", selected, moves.ToArray());
                moveProp.stringValue = moves[selected];
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif
