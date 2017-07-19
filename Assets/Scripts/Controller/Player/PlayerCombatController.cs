using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour {
    PlayerController playerController;
    PlayerStats playerStats;
    float currentGCD;

	// Use this for initialization
	void Start () {
        playerController = gameObject.GetComponent<PlayerController>();
        playerStats = gameObject.GetComponent<PlayerStats>();
        currentGCD = 0;
	}

    private void TurnToKeybindPush()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(hitPoint - transform.position);
        }
    }

    GameObject RaycastAtMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.SphereCast(ray, .5f, out hit, int.MaxValue, 1 << 11))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void CastSpell(PlayerSpells.SpellSlots slot)
    {
        playerController.isMoving = false;
        Spell castedSpell = playerController.spells.GetSelectedSpell(slot);
        if (playerStats.CurrentResource >= castedSpell.manaCost)
        {
            if (playerController.spells.IsSpellAvailable(slot) && (castedSpell.ignoreGCD || currentGCD == 0))
            {
                GameObject target = RaycastAtMouse();
                
                GameObject newSpell = Instantiate(Resources.Load(castedSpell.objectPath, typeof(GameObject)), gameObject.transform, false) as GameObject;
                newSpell.transform.position += (transform.forward * (castedSpell.range / 2));
                newSpell.GetComponent<Spell_GameObject>().SetSpell(playerStats, castedSpell, target);
                if (castedSpell.ignoreGCD == false)
                    currentGCD = playerStats.AttackSpeed;
                playerController.spells.PutSpellOnCD(slot);
                playerStats.CurrentResource -= castedSpell.manaCost;
            }
        }

        if (!Input.GetButton("Mouse PlayerMove"))
        {
            playerController.timeClickedOnEnemy = 0;
            playerController.inCombatMove = false;
            playerController.targettedEnemy = null;
        }
    }

    // Update is called once per frame
    void Update () {
        if (!GameVariables.isPaused)
        {
            if (Input.GetButton("Mouse PlayerMove") && playerController.ForceStop)
            {
                CastSpell(PlayerSpells.SpellSlots.LMB);
            }
            else if (playerController.inCombatMove && playerController.targettedEnemy)
            {
                Spell leftMouseSpell = playerController.spells.GetSelectedSpell(PlayerSpells.SpellSlots.LMB);
                Vector3 enemyLocation = playerController.targettedEnemy.transform.position;
                if ((gameObject.transform.position - enemyLocation).magnitude <= leftMouseSpell.range)
                {
                    CastSpell(PlayerSpells.SpellSlots.LMB);
                }
                else
                {
                    playerController.MovePlayer(playerController.targettedEnemy.transform.position);
                }
            }
            else if (playerController.inCombatMove)
            {
                playerController.targettedEnemy = null;
            }

            if (BindableInput.BindDown("Spell 1"))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.Slot1);
            }
            else if (BindableInput.BindDown("Spell 2"))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.Slot2);
            }
            else if (BindableInput.BindDown("Spell 3"))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.Slot3);
            }
            else if (BindableInput.BindDown("Spell 4"))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.Slot4);
            }
            else if (BindableInput.BindDown("Spell 5"))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.Slot5);
            }
            else if (Input.GetMouseButton(1))
            {
                TurnToKeybindPush();
                CastSpell(PlayerSpells.SpellSlots.RMB);
            }

            if (currentGCD != 0)
            {
                currentGCD -= Time.deltaTime;
                if (currentGCD < 0)
                    currentGCD = 0;
            }
        }
	}
}
