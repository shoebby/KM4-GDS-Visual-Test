using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    public KeyCode PullSelect;
    public KeyCode PushSelect;

    public GameObject PullGrenade;
    public GameObject PushGrenade;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PullSelect))
        {
            PullGrenade.transform.localScale = new Vector3(.6f, .6f, 1f);

            PushGrenade.transform.localScale = new Vector3(.5f, .5f, .5f);
        } 
        else if(Input.GetKeyDown(PushSelect))
        {
            PullGrenade.transform.localScale = new Vector3(.5f, .5f, .5f);

            PushGrenade.transform.localScale = new Vector3(.7f, .7f, .7f);
        }
    }
}
