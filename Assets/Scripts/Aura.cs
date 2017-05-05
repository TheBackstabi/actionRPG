using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AuraMods { Damage, Cost, Cooldown, Proc, NUM_MODS };

[System.Serializable]
public class Aura {
    [System.Serializable]
    public struct Modifier
    {
        public int spellID;
        public AuraMods mod;
        public float value;
    }
    public int id;
    public float duration;
    public bool consumedOnUse, isDebuff;
    public List<Modifier> mods;
    public byte[] textureData;
    public string auraName, desc;

    public Aura()
    {
        auraName = desc = "";
        duration = -1;
        id = 0;
        consumedOnUse = isDebuff = false;
        textureData = null;
        mods = new List<Modifier>();
    }

    public void DebugPrint()
    {
        int numMods = 1;
        string modifierString = "";
        foreach(Modifier mod in mods)
        {
            modifierString += "\nMod " + numMods + " -- spellID: " + mod.spellID + ", modType: " + mod.mod.ToString() + ", Value: " + mod.value;
        }
        Debug.Log(
            "ID: " + id.ToString() +
            ", name: \"" + auraName + "\"" +
            ", desc: \"" + desc + "\"" +
            ", oneUse: \"" + consumedOnUse.ToString() + "\"" +
            ", isDebuff: \"" + isDebuff.ToString() + "\"" +
            ", duration: \"" + duration.ToString() + "\"" +
            modifierString);

    }
}
