using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public Material blankMaterial;
    public Vector3 nextLevelPositionOffset;
    public bool loadNextLevelBelow;

    private static GameObject environmentCopy;
    private Collider coll;
    public string nextScene;
    private string currentScene;

    void Start()
    {
        // if we already have an active SceneTransitioner disable this one
        if (FindObjectsOfType<SceneTransitioner>().Length > 1)
        {
            GetComponentInChildren<MeshRenderer>().material = blankMaterial;
            Destroy(this);
        }

        coll = GetComponent<BoxCollider>();
        currentScene = SceneManager.GetActiveScene().name;

        if (SceneManager.sceneCount < 2 && loadNextLevelBelow)
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
            StartCoroutine(OffsetLoadedScene());
        }

        if (!environmentCopy)
        {
            Instantiate(FindObjectOfType<StayOnLoad>().gameObject, nextLevelPositionOffset, Quaternion.identity);
        }
        else
        {
            transform.position = nextLevelPositionOffset;
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
