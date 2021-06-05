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
    private int nextScene;
    private int currentScene;
    private int lastBuildIndex;

    void Start()
    {
        // if we already have an active SceneTransitioner disable this one
        if (FindObjectsOfType<SceneTransitioner>().Length > 1)
        {
            GetComponentInChildren<MeshRenderer>().material = blankMaterial;
            Destroy(this);
        }

        coll = GetComponent<BoxCollider>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        lastBuildIndex = SceneManager.sceneCountInBuildSettings - 1;

        if(SceneManager.GetActiveScene().buildIndex == lastBuildIndex)
        {
            nextScene = 0;
        }
        else
        {
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCount < 2 && loadNextLevelBelow)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
                StartCoroutine(OffsetLoadedScene());
            }

            if (!environmentCopy)
            {
                if (FindObjectOfType<StayOnLoad>())
                {
                    environmentCopy = Instantiate(FindObjectOfType<StayOnLoad>().gameObject, nextLevelPositionOffset, Quaternion.identity);
                    Destroy(environmentCopy.GetComponent<StayOnLoad>());
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ReloadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().ResetInputs();
            if(nextScene == 0)
            {
                Destroy(FindObjectOfType<StayOnLoad>().gameObject);
            }
            SceneManager.LoadScene(nextScene);
        }
    }

    public void ReloadScene()
    {
        foreach(PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.ResetInputs();
        }
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
