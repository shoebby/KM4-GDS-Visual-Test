using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BulletHitActivator : MonoBehaviour
{
    public UnityEvent hitEvent;
    public bool invokeOnCollision;
    public bool invokeOnTimer;
    public bool invokeOnRethrow;
    public bool left;
    public float timerDuration;

    private GameObject spawnInstance;
    private float startTime;

    private void Start()
    {
        if (invokeOnTimer)
        {
            startTime = Time.time;
        }
    }

    void Update()
    {
        if(invokeOnTimer && (startTime + timerDuration) < Time.time)
        {
            if (hitEvent == null) { return; }
            hitEvent.Invoke();
        }
    }

    public void Spawn(GameObject obj)
    {
        spawnInstance = Instantiate(obj, transform.position, Quaternion.identity);
    }

    public void GiveSpawnReferenceToPlayer()
    {
        if (left)
        {
            FindObjectOfType<PlayerController>().thrownLeft = spawnInstance;
        }
        else
        {
            FindObjectOfType<PlayerController>().thrownRight = spawnInstance;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitEvent == null || !invokeOnCollision || collision.gameObject.tag == "GravGrenade") { return; }
        hitEvent.Invoke();
    }
}
