using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_GameObject_Direct : Spell_GameObject {
    bool isFinished = false;

    IEnumerator DamageEnemies(Collider[] colliders)
    {
        foreach (Collider other in colliders)
        {
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
            if (other)
                other.gameObject.GetComponent<EnemyStats>().TakeDamage(damageDone, isCrit);
            yield return new WaitForSeconds(Time.deltaTime * .01f);
        }
        isFinished = true;
        yield return null;
    }

    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, spell.range/2, 1 << 11);
        StartCoroutine(DamageEnemies(colliders));
    }

    void Update()
    {
        if (isFinished)
            Destroy(gameObject);
    }
}
