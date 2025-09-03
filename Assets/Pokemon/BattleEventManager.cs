using System.Collections.Generic;

namespace PKMN
{
    public class BattleEventManager
    {
        private readonly Dictionary<BattleEvent, List<BattleEffect>> listeners = new();

        public void Subscribe(BattleEvent evt, BattleEffect effect)
        {
            if (!listeners.TryGetValue(evt, out var list))
            {
                list = new List<BattleEffect>();
                listeners[evt] = list;
            }
            if (effect != null && !list.Contains(effect))
                list.Add(effect);
        }

        public void Unsubscribe(BattleEvent evt, BattleEffect effect)
        {
            if (listeners.TryGetValue(evt, out var list))
                list.Remove(effect);
        }

        public void Raise(BattleEvent evt, BattlePokemon user, BattlePokemon target, MoveDefinition move, BattleContext context)
        {
            if (listeners.TryGetValue(evt, out var list))
            {
                foreach (var effect in list)
                    effect.Apply(user, target, move, context);
            }
        }
    }
}
