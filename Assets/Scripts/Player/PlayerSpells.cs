using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpells : MonoBehaviour {
    // TODO: Dump selected/available spells to file (Unity save game)
    public enum SpellSlots { Slot1, Slot2, Slot3, Slot4, Slot5, LMB, RMB, NUM_SLOTS };
    List<int> availableSpells;
    int[] selectedSpells;
    float[] spellCooldowns;

	// Use this for initialization
	void Start () {
        availableSpells = new List<int>();
        selectedSpells = new int[(int)SpellSlots.NUM_SLOTS];
        spellCooldowns = new float[(int)SpellSlots.NUM_SLOTS];
        availableSpells.Add(1);
        availableSpells.Add(2);
        availableSpells.Add(3);
        availableSpells.Add(4);
        selectedSpells[(int)SpellSlots.Slot1] = 2;
        selectedSpells[(int)SpellSlots.RMB] = 1;
        selectedSpells[(int)SpellSlots.Slot2] = 3;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < (int)SpellSlots.NUM_SLOTS; i++)
        {
            if (spellCooldowns[i] > 0)
            {
                spellCooldowns[i] -= Time.deltaTime;
                if (spellCooldowns[i] < 0)
                    spellCooldowns[i] = 0;
            }
        }
	}

    public void SetSelectedSpellID(SpellSlots slot, int spellID)
    {
        if (availableSpells.Contains(spellID))
        {
            selectedSpells[(int)slot] = spellID;
        }
    }

    public int GetSelectedSpellID(SpellSlots slot)
    {
        return selectedSpells[(int)slot];
    }

    public Spell GetSelectedSpell(SpellSlots slot)
    {
        return SpellDatabase.GetSpellData(selectedSpells[(int)slot]);
    }

    public bool IsSpellAvailable(SpellSlots slot)
    {
        if (spellCooldowns[(int)slot] == 0)
        {
            // TODO: Player stunned or otherwise unable to cast...?
            return true;
        }
        return false;
    }

    public void PutSpellOnCD(SpellSlots slot)
    {
        spellCooldowns[(int)slot] = SpellDatabase.GetSpellData(selectedSpells[(int)slot]).cooldown;
    }

    private void OnDestroy()
    {
        
    }
}
