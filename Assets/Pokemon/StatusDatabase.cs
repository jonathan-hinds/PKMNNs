using System.Collections.Generic;
using UnityEngine;

namespace PKMN
{
    [CreateAssetMenu(fileName = "StatusDatabase", menuName = "PKMN/Status Database")]
    public class StatusDatabase : ScriptableObject
    {
        public List<StatusDefinition> statuses;

        public StatusDefinition GetById(string id)
        {
            return statuses.Find(s => s.id == id);
        }
    }
}
