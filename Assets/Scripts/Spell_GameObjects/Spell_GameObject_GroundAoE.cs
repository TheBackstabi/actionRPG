using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_GameObject_GroundAoE : Spell_GameObject
{
    [SerializeField]
    bool isDoT;
    Vector3 centerPoint;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Tick(int ticksRemaining)
    {
        while (ticksRemaining > 0)
        {
            DealDamage();
            ticksRemaining--;
            yield return new WaitForSeconds(spell.duration*1.0f/spell.ticks);
        }
        Destroy(gameObject);
    }

    void DealDamage()
    {
        Collider[] enemies = Physics.OverlapSphere(centerPoint, spell.radius, 1 << 11);
        foreach (Collider enemy in enemies)
        {
            EnemyStats tarStats = enemy.gameObject.GetComponent<EnemyStats>();
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
        }
    }

    public override void SetSpell(PlayerStats _casterStats, Spell _spell, GameObject _target = null)
    {
        base.SetSpell(_casterStats, _spell, _target);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.SphereCast(ray, .5f, out hit, int.MaxValue, 1 << 0))
        {
            centerPoint = hit.point;
            if ((centerPoint - casterStats.transform.position).magnitude > spell.range)
            {
                centerPoint = transform.position;
            }
            transform.position = centerPoint;
            transform.parent = null;
            if (isDoT)
                StartCoroutine(Tick(spell.ticks));
            else
            {
                DealDamage();
                Destroy(gameObject);
            }
        }
        
    }
}
