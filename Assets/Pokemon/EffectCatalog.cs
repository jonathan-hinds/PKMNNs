using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectCatalogEntry
{
    public string id;
    [TextArea]
    public string argsJson;
}

[CreateAssetMenu(fileName = "EffectCatalog", menuName = "PKMN/Effect Catalog")]
public class EffectCatalog : ScriptableObject
{
    public List<EffectCatalogEntry> effectsCatalog;

    public EffectCatalogEntry GetById(string id)
    {
        return effectsCatalog.Find(e => e.id == id);
    }
}
