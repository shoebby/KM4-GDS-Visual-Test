using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathSurface : MonoBehaviour
{
    public GameObject sceneTransitioner;

    void Start()
    {
        sceneTransitioner = FindObjectOfType<SceneTransitioner>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sceneTransitioner.GetComponent<SceneTransitioner>().ReloadScene();
        }
    }
}
