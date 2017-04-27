using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_GameObject : MonoBehaviour {
    protected Spell spell = null;
    protected PlayerStats casterStats;
    protected GameObject target;

    public virtual void SetSpell(PlayerStats _casterStats, Spell _spell, GameObject _target = null)
    {
        casterStats = _casterStats;
        spell = _spell;
        target = _target;
    }
}
