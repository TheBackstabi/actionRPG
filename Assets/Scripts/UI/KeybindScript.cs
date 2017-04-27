using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindScript : MonoBehaviour {
    public Text nameText;
    public Button primaryBind;
    public Button altBind;

    IEnumerator SetKeybind(bool isAltBind)
    {
        FindObjectOfType<KeybindWarningScript>().ToggleText();
        bool isDone = false;
        while (true)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    FindObjectOfType<KeybindWarningScript>().ToggleText();
                    if (BindableInput.KeyAlreadyBound(vKey, nameText.text))
                    {
                        KeyAlreadyBound_Script.STATUS status;
                        Canvas menu = FindObjectOfType<GameVariables>().CreateMenu(GameVariables.MENU_TYPES.KeyAlreadyBound);
                        menu.GetComponent<KeyAlreadyBound_Script>().Bind = BindableInput.GetBindName(vKey);
                        do
                        {
                            status = menu.GetComponent<KeyAlreadyBound_Script>().GetCurrentStatus();
                            if (status == KeyAlreadyBound_Script.STATUS.Waiting)
                                yield return new WaitForEndOfFrame();
                        } while (status == KeyAlreadyBound_Script.STATUS.Waiting);
                        if (status == KeyAlreadyBound_Script.STATUS.Break)
                        {
                            isDone = true;
                            break;
                        }
                    }
                    isDone = true;
                    if (vKey == KeyCode.Escape || vKey == KeyCode.Mouse0 || vKey == KeyCode.Mouse1)
                        break;
                    if (!isAltBind)
                    {
                        if (vKey == KeyCode.Backspace)
                            BindableInput.UpdatePrimaryBind(nameText.text, KeyCode.None);
                        else
                            BindableInput.UpdatePrimaryBind(nameText.text, vKey);
                    }
                    else
                    {
                        if (vKey == KeyCode.Backspace)
                            BindableInput.UpdateAltBind(nameText.text, KeyCode.None);
                        else
                            BindableInput.UpdateAltBind(nameText.text, vKey);
                    }
                    isDone = true;
                    break;
                }
            }
            if (isDone)
                break;
            yield return new WaitForEndOfFrame();
        }
    }
	void Start()
    {
        
    }
	// Update is called once per frame
	void Update () {
        RefreshDisplay();
	}

    void RefreshDisplay()
    {
        primaryBind.GetComponentInChildren<Text>().text = BindableInput.GetBindString(nameText.text);
        altBind.GetComponentInChildren<Text>().text = BindableInput.GetAltBindString(nameText.text);
    }

    public void BUTTON_Primary()
    {
        StartCoroutine(SetKeybind(false));
    }

    public void BUTTON_Alt()
    {
        StartCoroutine(SetKeybind(true));
    }
}
