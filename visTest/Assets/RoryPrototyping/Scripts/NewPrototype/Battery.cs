using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float chargeTime;
    [HideInInspector] public bool filled;
    [HideInInspector] public int charges;
    [HideInInspector] public GameObject ballCharge;
    [HideInInspector] public ElectricBoxBehaviour ebb;

    private void Start()
    {
        ebb = GetComponentInParent<ElectricBoxBehaviour>();
    }

    private void Update()
    {
        if (filled && charges <= 0 && ebb.box.connectedRopeEnd)
        {
            ebb.box.connectedRopeEnd.ChargeStop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ChargeBallBehaviour>() && !filled)
        {
            ChargeBallBehaviour ball = other.GetComponent<ChargeBallBehaviour>();

            filled = true;
            ballCharge = other.gameObject;
            ball.move = true;
            ball.target = transform.position;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            StartCoroutine(startCharging());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ChargeBallBehaviour>() && filled)
        {
            filled = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public IEnumerator startCharging()
    {
        while (charges < ebb.chargedIndicators.Length)// && ballCharge.GetComponent<ChargeBallBehaviour>().charges > 0)
        {
            yield return new WaitForSeconds(chargeTime);

            ebb.chargedIndicators[charges].SetActive(true);
            charges++;

            if (ballCharge)
            {
                //ballCharge.GetComponent<ChargeBallBehaviour>().charges--;
            }

            if (ebb.box.filled && ebb.battery.charges > 0 && !ebb.box.connectedRopeEnd.incomingCharge)
            {
                ebb.box.connectedRopeEnd.ChargeStart();
            }
        }

        if (ballCharge)
        {
            if (ballCharge.GetComponent<ChargeBallBehaviour>().charges <= 0)
            {
                Destroy(ballCharge);
            }
        }
        yield return null;
    }
}
