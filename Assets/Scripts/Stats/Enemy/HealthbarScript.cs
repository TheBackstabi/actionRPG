using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour {
    [SerializeField]
    GUITexture foreground;
    [SerializeField]
    GUIText text;
    GUITexture background;

    GameObject target;
    EnemyStats targetStats;
    bool isReady = false;

	// Use this for initialization
	void Start () {
        background = GetComponent<GUITexture>();
	}

    public void Begin(GameObject _target, EnemyStats _targetStats)
    {
        target = _target;
        targetStats = _targetStats;
        isReady = true;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (isReady)
        {
            if (target != null)
            {
                Vector2 pos = targetStats.GetHealthBarPos();
                float width = targetStats.GetHealthBarSize();
                background.pixelInset = new Rect(pos.x - 3, pos.y - 3, 106, 6);
                foreground.pixelInset = new Rect(pos.x, pos.y, 100 * width, 0);
            }
        }
    }
}
