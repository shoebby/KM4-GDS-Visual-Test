using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StayOnLoad : MonoBehaviour
{
    void Start()
    {
        if (FindObjectsOfType<StayOnLoad>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }
}
