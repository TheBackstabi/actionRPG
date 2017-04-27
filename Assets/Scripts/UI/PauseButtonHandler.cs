using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BUTTON_ExitGame()
    {
        Application.Quit();
    }

    public void BUTTON_ReturnToGame()
    {
        GameVariables.UnPause();
    }

    public void BUTTON_KeybindMenu()
    {
        FindObjectOfType<GameVariables>().CreateMenu(GameVariables.MENU_TYPES.Keybind);
    }

    public void BUTTON_OptionsMenu()
    {
        FindObjectOfType<GameVariables>().CreateMenu(GameVariables.MENU_TYPES.Options);
    }
}
