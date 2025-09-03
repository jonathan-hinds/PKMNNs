using UnityEngine;

[System.Serializable]
public class EffectData
{
    // ID of the effect entry in the EffectCatalog
    public string effectId;

    // Optional JSON overrides for the catalog entry's arguments
    [TextArea]
    public string overrideArgsJson;
}
