using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBehaviour : MonoBehaviour
{
    public float lightTimeOfSingleCharge;
    public GameObject[] TurnOnObjects;
    ContactBox box;
    float lastTime;

    void Start()
    {
        box = GetComponentInChildren<ContactBox>();
        foreach(GameObject g in TurnOnObjects)
        {
            g.SetActive(false);
        }
    }

    private void Update()
    {
        if (connectedBatteryChargeLeft() > 0)
        {
            TurnOn();
        }
        else if (TurnOnObjects[0].activeSelf && lastTime < Time.time)
        {
            foreach (GameObject g in TurnOnObjects)
            {
                g.SetActive(false);
            }
        }
    }

    public void TurnOn()
    {
        if (connectedBatteryChargeLeft() > 0 && lastTime < Time.time)
        {
            if (box.connectedRopeEnd)
            {
                //returns Null
                Debug.Log(box.connectedRopeEnd.rope.ropeEndCap.box.electricBox);

                //box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery.charges--;
                //box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.UpdateIndicators();
                lastTime = Time.time + lightTimeOfSingleCharge;
                foreach (GameObject g in TurnOnObjects)
                {
                    g.SetActive(true);
                }
            }
            else
            {
                //box.connectedRopeEnd.rope.ropeStartCap.box.electricBox.battery.charges--;
                //box.connectedRopeEnd.rope.ropeStartCap.box.electricBox.UpdateIndicators();
                lastTime = Time.time + lightTimeOfSingleCharge;
                foreach (GameObject g in TurnOnObjects)
                {
                    g.SetActive(true);
                }
            }
        }
    }

    int connectedBatteryChargeLeft()
    {
        if (!box.connectedRopeEnd) { return -1; }
        if (box.connectedRopeEnd.ropeEnd)
        {
            if (box.connectedRopeEnd.rope.ropeStartCap.box)
            {
                if (box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery == null) { return -1; }
                if (box.connectedRopeEnd.rope.ropeStartCap.box.electricBox.battery)
                {
                    if (box.connectedRopeEnd.rope.ropeStartCap.box.electricBox.battery.charges > 0)
                    {
                        return box.connectedRopeEnd.rope.ropeStartCap.box.electricBox.battery.charges;
                    }
                    else { return 0; }
                }
                else { return -1; }
            }
            else { return -1; }
        }
        else
        {
            if (box.connectedRopeEnd.rope.ropeEndCap.box)
            {
                if(box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery == null) { return -1; }
                if (box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery)
                {
                    if (box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery.charges > 0)
                    {
                        return box.connectedRopeEnd.rope.ropeEndCap.box.electricBox.battery.charges;
                    }
                    else { return 0; }
                }
                else { return -1; }
            }
            else { return -1; }
        }
    }
}
