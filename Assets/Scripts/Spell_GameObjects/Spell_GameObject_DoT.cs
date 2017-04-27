using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_GameObject_DoT : Spell_GameObject {
    [SerializeField]
    bool dealDamageInstantly = false, spreadLikeCancer = false;
    List<Collider> spreadToTargets;

	IEnumerator Tick(int ticksRemaining)
    {
        if (!dealDamageInstantly)
            yield return new WaitForSeconds(spell.duration * 1.0f / spell.ticks);
        while (ticksRemaining > 0)
        {
            if (target == null)
                StopDoT();
            EnemyStats tarStats = target.GetComponent<EnemyStats>();
            bool isCrit = false;
            int damageDone;
            if (spell.school == 0)
                damageDone = casterStats.PhysicalDamage * spell.damageValue;
            else
                damageDone = casterStats.MagicalDamage * spell.damageValue;
            damageDone = GameVariables.RandomDamageRange(damageDone, .3f);
            float critChance = casterStats.CritChance / 100;
            float randomValue = (float)System.Math.Round(Random.Range(0f, 1f), 2);
            if (critChance >= randomValue)
            {
                isCrit = true;
                damageDone *= 2;
            }
            
            tarStats.TakeDamage(damageDone, isCrit);
            ticksRemaining--;
            if (spreadLikeCancer)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.parent.position, spell.range / 2, 1 << 11);
                foreach (Collider col in colliders)
                {
                    Spell_GameObject_DoT[] dots = col.gameObject.GetComponentsInChildren<Spell_GameObject_DoT>();
                    if (dots.Length == 0)
                    {
                        GameObject newSpell = Instantiate(Resources.Load(spell.objectPath, typeof(GameObject))) as GameObject;
                        newSpell.GetComponent<Spell_GameObject>().SetSpell(casterStats, spell, col.gameObject);
                        spreadLikeCancer = false;
                        break;
                    }
                    else
                    {
                        foreach (Spell_GameObject_DoT dot in dots)
                        {
                            if (dot.spell != spell)
                            {
                                GameObject newSpell = Instantiate(Resources.Load(spell.objectPath, typeof(GameObject))) as GameObject;
                                newSpell.GetComponent<Spell_GameObject>().SetSpell(casterStats, spell, col.gameObject);
                                spreadLikeCancer = false;
                                break;
                            }
                        }
                    }
                    if (!spreadLikeCancer)
                    {
                        Debug.Log("Spread");
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(spell.duration * 1.0f / spell.ticks);
        }
        StopDoT();
        yield return null;
    }

    public void StopDoT()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public override void SetSpell(PlayerStats _casterStats, Spell _spell, GameObject _target = null)
    {
        base.SetSpell(_casterStats, _spell, _target);
        if (target == null)
            StopDoT();
        else
        {
            transform.SetParent(target.transform, false);
            transform.localPosition = Vector3.zero;
            StartCoroutine(Tick(spell.ticks));
        }
    }
}
