using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPG_ResourceTypes { Mana, Rage, Souls, Energy, NUM_TYPES }

public static class RPG_Resource {
    public static string GetTypeName(this RPG_ResourceTypes T)
    {
        // TODO: Localization
        switch (T)
        {
            case RPG_ResourceTypes.Mana: return "Mana";
            case RPG_ResourceTypes.Rage: return "Rage";
            case RPG_ResourceTypes.Souls: return "Souls";
            case RPG_ResourceTypes.Energy: return "Blood";
            default: return "INVALID_TYPE";
        }
    }

    public static Color GetTypeColor(this RPG_ResourceTypes T)
    {
        switch (T)
        {
            case RPG_ResourceTypes.Mana: return Color.blue;
            case RPG_ResourceTypes.Rage: return new Color(1,(float)90/255, 0); //Orange
            case RPG_ResourceTypes.Souls: return Color.cyan;
            case RPG_ResourceTypes.Energy: return Color.yellow;
            default: return Color.magenta;
        }
    }
}
