using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindWarningScript : MonoBehaviour {
    [SerializeField]
    Text warningText;

	// Use this for initialization
	void Start () {
        warningText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleText()
    {
        warningText.enabled = !warningText.enabled;
    }

    public void BUTTON_Close()
    {
        GameVariables.CloseFrontmostMenu();
    }

    public void BUTTON_Defaults()
    {
        BindableInput.LoadDefaultBinds();
    }
}
