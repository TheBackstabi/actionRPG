using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour {
    [SerializeField]
    Text loadingText;
    private float sinceLastUpdate;
    private static float timeTillUpdate = .3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sinceLastUpdate += Time.deltaTime;
        if (sinceLastUpdate >= timeTillUpdate)
        {
            sinceLastUpdate = 0;
            loadingText.text += ".";
            if (loadingText.text.Length > 10)
                loadingText.text = "LOADING...";
        }
	}
}
