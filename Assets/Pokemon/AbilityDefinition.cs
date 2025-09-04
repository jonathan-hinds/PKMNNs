using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityDefinition
{
    public string id;
    public string name;
    [TextArea]
    public string summary;
    public List<EffectHook> hooks = new();
}
