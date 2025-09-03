using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusDefinition
{
    public string id;
    public int minDuration;
    public int maxDuration;
    public List<EffectHook> hooks;
}

// Database class moved to StatusDatabase.cs
