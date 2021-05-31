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
        if (!sceneTransitioner)
        {
            if (FindObjectOfType<SceneTransitioner>())
            {
                sceneTransitioner = FindObjectOfType<SceneTransitioner>().gameObject;
            }
            else
            {
                Destroy(gameObject);
            }
           
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sceneTransitioner.GetComponent<SceneTransitioner>().ReloadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sceneTransitioner.GetComponent<SceneTransitioner>().ReloadScene();
        }
    }
}
