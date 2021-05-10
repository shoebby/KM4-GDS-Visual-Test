using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBallBehaviour : MonoBehaviour
{
    public float speed;
    public int charges;
    [HideInInspector] public bool move;
    [HideInInspector] public Vector3 target;

    void Start()
    {
        move = false;
    }

    void Update()
    {
        if (move)
        {
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                move = false;
                transform.position = target;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime * speed);
        }
    }
}
