using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    protected PlayerStats player;
    protected PlayerSpells playerSpells;
    protected PlayerController playerController;
    [SerializeField]
    protected Text hpDisplay, mpDisplay, xpDisplay;
    [SerializeField]
    protected Image hpBar, mpBar, xpBar;
    [SerializeField]
    protected Text spell1Bind, spell2Bind, spell3Bind, spell4Bind, spell5Bind, currentLevel, resets;
    protected Texture2D spell1tex, spell2tex, spell3tex, spell4tex, spell5tex, spellLtex, spellRtex;
    [SerializeField]
    protected GameObject resetsObject;
    [SerializeField]
    protected Text warningText;
    [SerializeField]
    protected Button spell1, spell2, spell3, spell4, spell5, spellL, spellR;
    [SerializeField]
    protected Text FPS;
    protected ActionButtonScript s1script, s2script, s3script, s4script, s5script, sLscript, sRscript;

    private bool hasBeenInit = false;
    private static UIController instance = null;
    public static UIController Instance
    {
        get { return instance; }
    }

    Queue<int> fpsArray;

    void Awake()
    {
        instance = this;
    }

    public static UIController GetInstance()
    {
        return Instance;
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        if (!s1script)
        {
            s1script = spell1.GetComponent<ActionButtonScript>();
            s2script = spell2.GetComponent<ActionButtonScript>();
            s3script = spell3.GetComponent<ActionButtonScript>();
            s4script = spell4.GetComponent<ActionButtonScript>();
            s5script = spell5.GetComponent<ActionButtonScript>();
            sLscript = spellL.GetComponent<ActionButtonScript>();
            sRscript = spellR.GetComponent<ActionButtonScript>();
        }

        fpsArray = new Queue<int>();
    }

    public void Begin(PlayerStats _player, PlayerSpells _spells, PlayerController _controller)
    {
        if (!hasBeenInit)
        {
            player = _player;
            playerSpells = _spells;
            playerController = _controller;
            mpBar.color = player.ResourceType.GetTypeColor();
            UpdateActionbar();
            UpdateBindText();
            UpdateHealthAndResource();
            UpdateXP();
            UpdateLevelAndResets();
            hasBeenInit = true;
        }
    }

    public void UpdateActionbar()
    {
        if (!s1script)
        {
            s1script = spell1.GetComponent<ActionButtonScript>();
            s2script = spell2.GetComponent<ActionButtonScript>();
            s3script = spell3.GetComponent<ActionButtonScript>();
            s4script = spell4.GetComponent<ActionButtonScript>();
            s5script = spell5.GetComponent<ActionButtonScript>();
            sLscript = spellL.GetComponent<ActionButtonScript>();
            sRscript = spellR.GetComponent<ActionButtonScript>();
        }
        s1script.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.Slot1));
        s2script.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.Slot2));
        s3script.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.Slot3));
        s4script.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.Slot4));
        s5script.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.Slot5));
        sLscript.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.LMB));
        sRscript.SetSpell(playerSpells.GetSelectedSpell(PlayerSpells.SpellSlots.RMB));
    }

    public void UpdateBindText()
    {
        spell1Bind.text = BindableInput.GetBindString("Spell 1");
        spell2Bind.text = BindableInput.GetBindString("Spell 2");
        spell3Bind.text = BindableInput.GetBindString("Spell 3");
        spell4Bind.text = BindableInput.GetBindString("Spell 4");
        spell5Bind.text = BindableInput.GetBindString("Spell 5");
    }

    public void UpdateHealthAndResource()
    {
        hpDisplay.text = player.CurrentHealth.ToString();
        mpDisplay.text = player.CurrentResource.ToString();
        hpBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)player.CurrentHealth / player.MaxHealth * 100);
        mpBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)player.CurrentResource / player.MaxResource * 100);
    }

    public void UpdateXP()
    {
        xpDisplay.text = player.CurrentXp.ToString() + " / " + player.MaxXp + " (" + System.Math.Round(((float)player.CurrentXp / player.MaxXp * 100), 2) + "%)";
        xpBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)player.CurrentXp / player.MaxXp * 426);
    }

    public void UpdateLevelAndResets()
    {
        currentLevel.text = player.Level.ToString();
        if (player.Resets > 0)
        {
            resetsObject.SetActive(true);
            resets.text = player.Resets.ToString();
        }
        else
            resetsObject.SetActive(false);
    }

    void Update()
    {
        if (!GameVariables.isPaused)
        {
            if (BindableInput.BindDown("Spell 1", true))
                spell1.image.color = spell1.colors.pressedColor;
            else
                spell1.image.color = spell1.colors.normalColor;

            if (BindableInput.BindDown("Spell 2", true))
                spell2.image.color = spell2.colors.pressedColor;
            else
                spell2.image.color = spell2.colors.normalColor;

            if (BindableInput.BindDown("Spell 3", true))
                spell3.image.color = spell3.colors.pressedColor;
            else
                spell3.image.color = spell3.colors.normalColor;

            if (BindableInput.BindDown("Spell 4", true))
                spell4.image.color = spell4.colors.pressedColor;
            else
                spell4.image.color = spell4.colors.normalColor;

            if (BindableInput.BindDown("Spell 5", true))
                spell5.image.color = spell5.colors.pressedColor;
            else
                spell5.image.color = spell5.colors.normalColor;

            if (Input.GetButton("Mouse PlayerMove") && playerController.ForceStop || playerController.targettedEnemy)
                spellL.image.color = spellL.colors.pressedColor;
            else
                spellL.image.color = spellL.colors.normalColor;

            if (Input.GetMouseButton(1))
                spellR.image.color = spellR.colors.pressedColor;
            else
                spellR.image.color = spellR.colors.normalColor;

            if (BindableInput.BindDown("Character"))
            {
                Canvas createdCanvas = FindObjectOfType<GameVariables>().CreateMenu(GameVariables.MENU_TYPES.Character);
                if (createdCanvas)
                    createdCanvas.GetComponent<CharScreenStats>().Begin(player);
            }

            if (Time.deltaTime > 0)
            {
                fpsArray.Enqueue((int)(1 / Time.deltaTime));
                if (fpsArray.Count == 100)
                {
                    int avgFPS = 0;
                    foreach (int fps in fpsArray)
                        avgFPS += fps;
                    avgFPS /= 100;
                    FPS.text = "FPS: " + avgFPS.ToString();
                    fpsArray.Clear();
                }
            }
        }
    }

    IEnumerator WarningTextFade(float duration)
    {
        float curTimer = duration;
        while (curTimer > 0)
        {
            duration -= (Time.timeScale * .1f);
            Color curColor = warningText.color;
            curColor.a -= (1 - (curTimer / duration));
            warningText.color = curColor;
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    public void DrawWarning(string text, float duration = 5.0f)
    {
        warningText.text = text;
        warningText.color = Color.red;
        StartCoroutine(WarningTextFade(duration));
    }
}
