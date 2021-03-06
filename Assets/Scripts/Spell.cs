﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell {
    public string name, desc, objectPath, type, texture;
    public int id, manaCost, spellType, damageValue, range, school, cooldown, duration, radius, ticks;
    public bool ignoreGCD;
    
    public Spell()
    {
        name = desc = objectPath = texture = "";
        id = manaCost = spellType = damageValue = range = school = cooldown = duration = radius = ticks = 0;
    }

    public void DebugPrint()
    {
        Debug.Log(
            "id: " + id.ToString() + 
            ", name: \"" + name + "\"" + 
            ", desc: \"" + desc + "\"" + 
            ", manaCost: " + manaCost.ToString() + 
            ", spellType: " + spellType.ToString() + 
            ", damageValue: " + damageValue.ToString() +
            ", objectPath: " + objectPath);
    }
}
