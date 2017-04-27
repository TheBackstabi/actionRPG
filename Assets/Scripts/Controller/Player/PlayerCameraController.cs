using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    [SerializeField]
    protected Vector3 angle, distance;
    public float followRatio = 1.0f;
    public GameObject target;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        target = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 targetPos = target.transform.position + distance;
        Vector3 lineToTarget = targetPos - transform.position;
        Vector3 followRatioPosition = targetPos - (lineToTarget * (1-followRatio));
        transform.position = followRatioPosition;
        transform.rotation = Quaternion.Euler(angle);
	}
}
