using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{

    [SerializeField] GameObject partPrefab, parentObject, startCap, endCap;

    [SerializeField] [Range(1, 1000)] int length = 1;

    [SerializeField] float partDistance = 0.21f;

    [SerializeField] bool snapFirst, snapLast;

    [HideInInspector] GameObject[] ropeParts;

    private void Start()
    {
        Spawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Transform parent = Instantiate(parentObject, transform.position, Quaternion.identity).transform;

        int count = (int)(length / (partDistance * parent.localScale.y));

        ropeParts = new GameObject[count];

        for (int x = 0; x < count; x++)
        {
            GameObject tmp;

            tmp = ropeParts[x] = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y - (partDistance * parent.localScale.y) * (x + 1), transform.position.z), Quaternion.identity, parent);
            tmp.transform.eulerAngles = new Vector3(180, 0, 0);

            tmp.name = parent.childCount.ToString();

            if (x == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
                if (snapFirst) { tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; }
                if (startCap)
                {

                    RopeEndBehaviour cap = Instantiate(endCap, tmp.transform.position, Quaternion.identity, tmp.transform).GetComponent<RopeEndBehaviour>();
                    parent.GetComponent<RopeLineRenderer>().ropeStartCap = cap;
                    cap.ropeEnd = false;
                    cap.rope = parent.GetComponent<RopeLineRenderer>();
                }
            }
            else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = parent.Find((parent.childCount - 1).ToString()).GetComponent<Rigidbody>();

                if (partPrefab.GetComponent<SpringJoint>())
                {
                    tmp.GetComponent<SpringJoint>().connectedBody = parent.Find((parent.childCount - 1).ToString()).GetComponent<Rigidbody>();
                }
            }

            if (x == count - 1)
            {
                if (snapLast) { tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; }
                if (endCap)
                {
                    RopeEndBehaviour cap = Instantiate(endCap, tmp.transform.position, Quaternion.identity, tmp.transform).GetComponent<RopeEndBehaviour>();
                    parent.GetComponent<RopeLineRenderer>().ropeEndCap = cap;
                    cap.ropeEnd = true;
                    cap.rope = parent.GetComponent<RopeLineRenderer>();
                }
            }
        }

        if (parent.GetComponent<RopeLineRenderer>())
        {
            RopeLineRenderer rlr = parent.gameObject.GetComponent<RopeLineRenderer>();
            rlr.ropeParts = ropeParts;
            rlr.ropePartCount = ropeParts.Length;
            rlr.ropeRenderScale = parent.localScale;
        }
    }
}