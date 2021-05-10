using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBox : MonoBehaviour
{
    [HideInInspector] public bool filled;
    [HideInInspector] public ElectricBoxBehaviour electricBox;
    [HideInInspector] public LampBehaviour lamp;
    [HideInInspector] public RopeEndBehaviour connectedRopeEnd;

    private void Start()
    {
        electricBox = GetComponentInParent<ElectricBoxBehaviour>();
        lamp = GetComponentInParent<LampBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RopeEndBehaviour>() && !filled)
        {
            RopeEndBehaviour cap = other.GetComponent<RopeEndBehaviour>();

            filled = true;
            cap.move = true;
            cap.target = transform.position;
            cap.rot = transform.rotation;
            cap.box = this;
            other.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            connectedRopeEnd = cap;

            if (electricBox)
            {
                if (electricBox.battery.charges > 0 && !cap.incomingCharge)
                {
                    cap.ChargeStart();
                }
            }

            if (lamp && connectedRopeEnd.outcomingCharge)
            {
                lamp.TurnOn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RopeEndBehaviour>() && filled)
        {
            filled = false;
            other.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            connectedRopeEnd.box = null;
            
            if (connectedRopeEnd.incomingCharge)
            {
                connectedRopeEnd.chargedEffect.Stop();
                connectedRopeEnd.incomingCharge = false;
                if (connectedRopeEnd.ropeEnd)
                {
                    connectedRopeEnd.rope.ropeStartCap.outcomingCharge = false;
                    connectedRopeEnd.rope.ropeStartCap.chargedEffect.Stop();
                }
                else
                {
                    connectedRopeEnd.rope.ropeEndCap.outcomingCharge = false;
                    connectedRopeEnd.rope.ropeEndCap.chargedEffect.Stop();
                }
            }
            connectedRopeEnd = null;
        }
    }
}
