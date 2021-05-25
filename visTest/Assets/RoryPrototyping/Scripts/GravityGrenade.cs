using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGrenade : MonoBehaviour
{
    public float gravityForce;
    public float minDistForGravity;
    public float radius;
    public bool overTime;

    private void OnTriggerStay(Collider other)
    {
        if (!overTime) { return; }
        if (GetComponent<SphereCollider>())
        {
            if (GetComponent<SphereCollider>().isTrigger)
            {
                if (!other.gameObject.GetComponentInParent<Rigidbody>()) { return;  }

                if ((transform.position - other.transform.position).magnitude > minDistForGravity)
                {
                    //other.gameObject.GetComponentInParent<Rigidbody>().AddForce((transform.position - other.transform.position).normalized * gravityForce);
                }
            }
        }
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Rigidbody>())
            {
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce((transform.position - hitCollider.transform.position).normalized * gravityForce);
            }
        }
    }
}
