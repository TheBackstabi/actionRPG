using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStartScript : MonoBehaviour {
    [SerializeField]
    protected GameObject playerToInstantiate, UItoInstantiate;

	// Use this for initialization
	void Start () {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player instantiated via PlayerStart on scene " + SceneManager.GetActiveScene().name);
            player = Instantiate(playerToInstantiate, gameObject.transform.position, Quaternion.identity);
            Instantiate(UItoInstantiate);
        }
        player.GetComponent<PlayerController>().PlayerPosition = gameObject.transform.position;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
