using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCube : MonoBehaviour
{
    public float distance;
    public float height;
    Vector3 s;
    GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        s = transform.localScale;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) > distance)
        {
            transform.localScale = new Vector3(s.x, s.y, s.z);
        }
        else
        {
            transform.localScale = new Vector3(s.x, map(Vector3.Distance(transform.position, player.transform.position), 0, distance, height, s.y), s.z);
        }
    }

    private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
