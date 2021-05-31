using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public Vector3 nextLevelPositionOffset;
    public bool loadNextLevelBelow;

    private Collider coll;
    public string nextScene;
    private string currentScene;

    void Start()
    {
        coll = GetComponent<BoxCollider>();
        currentScene = SceneManager.GetActiveScene().name;

        if (SceneManager.sceneCount < 2 && loadNextLevelBelow)
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
            StartCoroutine(OffsetLoadedScene());
        }
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
            other.GetComponent<PlayerController>().ResetInputs();
            SceneManager.LoadScene(nextScene);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    IEnumerator OffsetLoadedScene()
    {
        yield return 0;
        foreach(GameObject obj in SceneManager.GetSceneAt(SceneManager.sceneCount - 1).GetRootGameObjects())
        {
            obj.transform.position += nextLevelPositionOffset;
        }
        
    }
}
