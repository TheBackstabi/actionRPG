using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCombatText : MonoBehaviour {
    public bool isActive = false;
    bool isCrit;
    public string text;
    float yPos = 20;
    float aliveTime = 0;
    GUIStyle style;

	// Use this for initialization
	void Start () {
        style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        if (isCrit)
        {
            style.fontSize = 22;
            style.normal.textColor = new Color(1, .4f, 0);
        }
        else
        {
            style.fontSize = 16;
            style.normal.textColor = Color.cyan;
        }
        isActive = true;
    }

    public void Create(Transform parent, string _text, bool _isCrit)
    {
        text = _text;
        transform.position = parent.transform.position;
        isCrit = _isCrit;
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            style.normal.textColor = new Color(style.normal.textColor.r, style.normal.textColor.g, style.normal.textColor.b, 1 - aliveTime);
            yPos += Time.deltaTime*20;
            aliveTime += Time.deltaTime;
            if (aliveTime >= 2.0f)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnGUI()
    {
        if (!GameVariables.isPaused && isActive)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            pos.y += yPos;
            Rect drawRect = new Rect( (pos.x - text.Length * 5f ), (Screen.height - pos.y - (Screen.height/15)), 100, 30);
            GUI.Label(drawRect, text, style);
        }
    }
}
