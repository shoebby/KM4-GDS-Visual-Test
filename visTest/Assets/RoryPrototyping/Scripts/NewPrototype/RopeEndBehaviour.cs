using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeEndBehaviour : MonoBehaviour
{
    public float speed;
    [HideInInspector] public bool ropeEnd;
    [HideInInspector] public bool move;
    [HideInInspector] public bool incomingCharge, outcomingCharge;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public Quaternion rot;
    [HideInInspector] public RopeLineRenderer rope;
    [HideInInspector] public ParticleSystem chargedEffect;
    [HideInInspector] public ContactBox box;

    void Start()
    {
        move = false;
        chargedEffect = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (!rope)
        {
            rope = transform.parent.GetComponentInParent<RopeLineRenderer>();
        }

        if (move)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        if (Vector3.Distance(transform.parent.position, target) < 0.01f)
        {
            move = false;
            transform.parent.position = target;
            transform.parent.rotation = rot;
            transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        transform.parent.position = Vector3.Slerp(transform.parent.position, target, Time.deltaTime * speed);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rot, Time.deltaTime * speed);
    }

    public void ChargeStart()
    {
        chargedEffect.Play();
        incomingCharge = true;
        if (ropeEnd)
        {
            rope.ropeStartCap.chargedEffect.Play();
            rope.ropeStartCap.outcomingCharge = true;
        }
        else
        {
            rope.ropeEndCap.chargedEffect.Play();
            rope.ropeEndCap.outcomingCharge = true;
        }
    }

    public void ChargeStop()
    {
        chargedEffect.Stop();
        incomingCharge = false;
        if (ropeEnd)
        {
            rope.ropeStartCap.chargedEffect.Stop();
            rope.ropeStartCap.outcomingCharge = false;
        }
        else
        {
            rope.ropeEndCap.chargedEffect.Stop();
            rope.ropeEndCap.outcomingCharge = false;
        }
    }
}
