using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGrenade : MonoBehaviour
{
    public float gravityForce;
    public float minDistForGravity;

    private void OnTriggerStay(Collider other)
    {
        if (GetComponent<SphereCollider>())
        {
            if (GetComponent<SphereCollider>().isTrigger)
            {
                if (!other.gameObject.GetComponentInParent<Rigidbody>()) { return;  }

                if ((transform.position - other.transform.position).magnitude > minDistForGravity)
                {
                    other.gameObject.GetComponentInParent<Rigidbody>().AddForce((transform.position - other.transform.position).normalized * gravityForce);
                }
            }
        }
    }
}
