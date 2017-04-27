using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonScript : MonoBehaviour {

    [SerializeField]
    RawImage spellIcon;

    Texture2D spellTexture;
    Button spellButton;
    GameObject tooltip;
    Spell setSpell;

    bool isReady = false;
    
	// Use this for initialization
	void Start () {
        spellTexture = new Texture2D(2, 2);
        spellButton = GetComponent<Button>();
        isReady = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        spellIcon.color = spellButton.image.color;
    }

    public void SetSpell(Spell selectedSpell)
    {
        if (!isReady)
            Start();
        setSpell = selectedSpell;
        spellTexture.LoadImage(selectedSpell.textureData);
        spellIcon.texture = spellTexture;
    }

    public void BUTTON_StartDrag()
    {
        //Drag button?
    }

    public void BUTTON_CreateTooltip()
    {
        RectTransform trans = GetComponent<RectTransform>();
        Vector2 pos = trans.position;
        pos.y += trans.rect.height/2;
        string potency = setSpell.damageValue.ToString() + "00% ";
        switch (setSpell.damageValue)
        {
            case 1: potency += "Physical Damage"; break;
            default: potency += "Magical Damage"; break;
        }
        tooltip = FindObjectOfType<GameVariables>().CreateTooltip(setSpell.name, setSpell.manaCost.ToString() + " Mana - " + potency, setSpell.desc, pos);
    }

    public void BUTTON_DestroyTooltip()
    {
        if(tooltip)
            Destroy(tooltip);
        tooltip = null;
    }
}
