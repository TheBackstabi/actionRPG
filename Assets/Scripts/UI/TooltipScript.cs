using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour {
    [SerializeField]
    protected Text header, subheader, body;

	// Use this for initialization
	void Start () {
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTooltip(string _header, string _subheader, string _body, Vector2 loc)
    {
        header.text = _header;
        subheader.text = _subheader;
        body.text = _body;
        transform.position = loc;
        this.enabled = true;
    }
}
