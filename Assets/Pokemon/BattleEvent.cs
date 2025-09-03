using System;

namespace PKMN
{
    public enum BattleEvent
    {
        TurnStart,
        BeforeMove,
        AfterMove,
        CalculateDamage,
        WeatherChanged,
        TurnEnd
    }
}
