using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAlreadyBound_Script : MonoBehaviour {
    public enum STATUS { Waiting, Continue, Break, Off }
    STATUS currentStatus;
    [SerializeField]
    Text bindText;
    string bind;
    public string Bind
    {
        set
        {
            bind = value;
        }
    }

    void Start()
    {
        currentStatus = STATUS.Waiting;
    }

    public void BUTTON_Yes()
    {
        currentStatus = STATUS.Continue;
    }

    public void BUTTON_No()
    {
        currentStatus = STATUS.Break;
    }

    void Update()
    {
        bindText.text = bind;
        if (currentStatus == STATUS.Off)
            Destroy(this.gameObject);
    }

    public STATUS GetCurrentStatus()
    {
        STATUS returnedStatus = currentStatus;
        if (currentStatus == STATUS.Continue || currentStatus == STATUS.Break)
            currentStatus = STATUS.Off;
        return returnedStatus;
    }
}
