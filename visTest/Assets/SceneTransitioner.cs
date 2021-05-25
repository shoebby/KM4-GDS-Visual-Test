using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private Collider coll;
    public string nextScene;
    public string currentScene;

    void Start()
    {
        coll = GetComponent<BoxCollider>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(currentScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
