using System.Collections;
using UnityEngine;

public class JumpPadControl : MonoBehaviour
{
    public float launchForce = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Rigidbody>()) { return; }
        other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }
}
