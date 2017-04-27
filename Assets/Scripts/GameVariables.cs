using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour {
    public enum MENU_TYPES { Keybind, Options, Character, KeyAlreadyBound };
    [SerializeField]
    GameObject spawnedHealthBar;
    [SerializeField]
    Canvas pauseMenu;
    static Canvas openedPauseMenu;
    [SerializeField]
    Canvas keybindMenu;
    [SerializeField]
    Canvas ERROR_KeyAlreadyBound;
    [SerializeField]
    Canvas characterScreen;
    [SerializeField]
    GameObject tooltipObject;
    public static Stack<Canvas> activeMenus;
    public static bool isPaused = false;
    public static int maxPlayerLevel = 50;
    public static int highestPlayerLevel;
    public static string locale = "enUS";

    public static void UpdateVariables()
    {
        highestPlayerLevel = 1;
        PlayerStats[] allPlayers = FindObjectsOfType<PlayerStats>();
        foreach (PlayerStats player in allPlayers)
        {
            if ( player.Level > highestPlayerLevel )
            {
                highestPlayerLevel = player.Level;
            }
        }
    }

	// Use this for initialization
	void Start () {
        UpdateVariables();
        activeMenus = new Stack<Canvas>();
    }

    public static void UnPause()
    {
        if (openedPauseMenu)
        {
            Destroy(openedPauseMenu.gameObject);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (BindableInput.BindDown("Close Menus"))
        {
            if (!isPaused)
                CloseAllMenus();
        }
		if (BindableInput.BindDown("Pause"))
        {
            if(activeMenus.Count > 0)
            {
                CloseAllMenus();
            }
            else
            {
                if (!isPaused)
                {
                    openedPauseMenu = Instantiate(pauseMenu);
                    Time.timeScale = 0;
                    isPaused = true;
                }
                else
                {
                    Destroy(openedPauseMenu.gameObject);
                    Time.timeScale = 1;
                    isPaused = false;
                }
            }
        }
	}

    public static int RandomDamageRange(int originalValue, float modulationRange)
    {
        float addToRange = originalValue * modulationRange;
        return originalValue + (int)Random.Range(addToRange, -addToRange);
    }

    public Canvas CreateMenu(MENU_TYPES type)
    {
        Canvas newCanvas = null;
        switch (type)
        {
            case MENU_TYPES.Keybind:
                newCanvas = Instantiate(keybindMenu);
                foreach (Canvas item in activeMenus)
                    item.enabled = false;
                activeMenus.Push(newCanvas);
                return newCanvas;
            case MENU_TYPES.KeyAlreadyBound:
                newCanvas = Instantiate(ERROR_KeyAlreadyBound);
                return newCanvas;
            case MENU_TYPES.Character:
                if (!RemoveSpecificMenu("CharScreen"))
                {
                    newCanvas = Instantiate(characterScreen);
                    activeMenus.Push(newCanvas);
                }
                return newCanvas;
            default:
                Debug.Log("GameVariables::CreateMenu -> Menu type \"" + type.ToString() + "\" is NYI");
                return null;
        }
    }

    public static void CloseAllMenus()
    {
        while (activeMenus.Count > 0)
        {
            Destroy(activeMenus.Pop().gameObject);
        }
    }

    public static void CloseFrontmostMenu()
    {
        if(activeMenus.Count > 0)
            Destroy(activeMenus.Pop().gameObject);
    }

    public static bool RemoveSpecificMenu(string menuName)
    {
        List<Canvas> array = new List<Canvas>(activeMenus.ToArray());
        for(int i = 0; i < activeMenus.Count; i++)
        {
            if (array[i].name.Contains(menuName))
            {
                DestroyImmediate(array[i].gameObject);
                array.RemoveAt(i);
                activeMenus = new Stack<Canvas>(array);
                return true;
            }
        }
        return false;
    }

    public GameObject CreateHealthbar(GameObject target)
    {
        GameObject bar = Instantiate(spawnedHealthBar);
        bar.GetComponent<HealthbarScript>().Begin(target, target.GetComponent<EnemyStats>());
        return bar;
    }

    public GameObject CreateTooltip(string header, string subheader, string body, Vector2 pos)
    {
        GameObject tooltip = Instantiate(tooltipObject);
        tooltip.GetComponentInChildren<TooltipScript>().SetTooltip(header, subheader, body, pos);
        return tooltip;
    }
}
