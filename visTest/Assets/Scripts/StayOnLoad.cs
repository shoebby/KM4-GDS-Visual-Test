using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnLoad : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
