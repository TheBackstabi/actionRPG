using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharScreenStats : MonoBehaviour {
    bool isReady = false;
    public Text charName, level, str, intel, agi, res, sta, vit;
    public Text phys, magi;
    public GameObject resetObject, currentTooltip;
    PlayerStats stats;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        GetComponent<Canvas>().enabled = false;
	}

    private void OnDestroy()
    {
        DeleteTooltip();
    }

    void ShowStats()
    {
        if (isReady)
        {
            charName.text = stats.CharName;
            level.text = stats.Level.ToString();
            str.text = stats.Strength.ToString();
            intel.text = stats.Intelligence.ToString();
            agi.text = stats.Agility.ToString();
            res.text = stats.Resistance.ToString();
            sta.text = stats.Stamina.ToString();
            vit.text = stats.Vitality.ToString();
            phys.text = stats.PhysicalDamage.ToString();
            magi.text = stats.MagicalDamage.ToString();
        }
    }

    public void Begin(PlayerStats playerToLoad)
    {
        stats = playerToLoad;
        isReady = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isReady)
        {
            GetComponent<Canvas>().enabled = true;
            ShowStats();
            if (stats.CanReset())
                resetObject.SetActive(true);
            else
                resetObject.SetActive(false);
            if (BindableInput.BindDown("Character"))
                Destroy(this.gameObject);
        }
	}

    public void BUTTON_LevelReset()
    {
        stats.ResetLevel();
    }

    public void STR_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = str.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("STR", "+" + stats.StrIncrement.ToString() + " per level", "Boosts physical damage and resistance.", pos);
        }
    }

    public void INT_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = intel.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("INT", "+" + stats.IntIncrement.ToString() + " per level", "Boosts magical damage and max resource pool. Also boosts resistance, but less effectively than strength.", pos);
        }
    }

    public void AGI_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = agi.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("AGI", "+" + stats.AgiIncrement.ToString() + " per level", "Boosts physical and magical damage, crit chance, and dodge chance.", pos);
        }
    }

    public void RES_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = res.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("RES", "+" + stats.ResIncrement.ToString() + " per level", "Reduces incoming damage. Effectiveness per point determined by level.", pos);
        }
    }

    public void STA_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = sta.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("STA", "+" + stats.StaIncrement.ToString() + " per level", "Increases health and resource regeneration.", pos);
        }
    }

    public void VIT_CreateTooltip()
    {
        if (!currentTooltip)
        {
            RectTransform trans = vit.GetComponent<RectTransform>();
            Vector2 pos = trans.position;
            pos.x = trans.parent.position.x + trans.rect.width;
            pos.y = trans.position.y;
            currentTooltip = FindObjectOfType<GameVariables>().CreateTooltip("VIT", "+" + stats.StrIncrement.ToString() + " per level", "Increases health pool.", pos);
        }
    }

    public void DeleteTooltip()
    {
        if(currentTooltip)
            Destroy(currentTooltip);
        currentTooltip = null;
    }
}
