using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnHit : MonoBehaviour
{
    public ParticleSystem hitFx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude > 5)
        {
            Instantiate(hitFx, collision.GetContact(0).point, Quaternion.identity);
        }
    }
}
