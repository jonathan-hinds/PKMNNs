using System.Collections.Generic;
using UnityEngine;

namespace PKMN
{
    [System.Serializable]
    public class StatusDefinition
    {
        public string id;
        public int minDuration;
        public int maxDuration;
        public List<EffectHook> hooks = new();
    }
}

// Database class moved to StatusDatabase.cs
