using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

    [SerializeField]
    private string sceneToLoad;
    [SerializeField]
    private GameObject canvasToShow;
    private GameObject loadingCanvas;
    private bool isUsable = false;
    private bool isLoading = false;

    IEnumerator LoadScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!load.isDone)
            yield return null;
        Destroy(loadingCanvas);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isUsable = true;
            if(!Input.GetButton("Mouse PlayerMove"))
            {
                isLoading = true;
                loadingCanvas = Instantiate(canvasToShow);
                other.gameObject.GetComponent<PlayerController>().isMoving = false;
                StartCoroutine(LoadScene());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isUsable = false;
    }

    private void OnMouseDown()
    {
        if (isUsable && !isLoading)
        {
            isLoading = true;
            loadingCanvas = Instantiate(canvasToShow);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().StopAllCoroutines();
            StartCoroutine(LoadScene());
        }
    }
}
