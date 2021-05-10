using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLineRenderer : MonoBehaviour
{
    public bool createRenderInstances;
    public GameObject ropePartRenderer;
    public int ropePartRenderSteps;

    [HideInInspector] public int ropePartCount;
    [HideInInspector] public Vector3 ropeRenderScale;
    [HideInInspector] public GameObject[] ropeParts;
    [HideInInspector] public GameObject[] renderInstances;
    [HideInInspector] public LineRenderer lr;
    [HideInInspector] public RopeEndBehaviour ropeStartCap, ropeEndCap;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (createRenderInstances)
        {
            if(ropePartCount > 0 && renderInstances.Length < ropePartCount)
            {
                CreateRenderInstances();
            }

            if (renderInstances.Length >= ropePartCount)
            {
                UpdateInstancePositions();
            }
        }

        if (!lr) { return; }
        if (!lr.enabled) { return; }
        else if(ropePartCount > 0 && lr.positionCount < ropePartCount) { lr.positionCount = ropePartCount; }
        for(int i = 0; i < ropeParts.Length; i++)
        {
            lr.SetPosition(i, ropeParts[i].transform.position);
        }
    }

    void UpdateInstancePositions()
    {
        for (int x = 0; x < ropePartRenderSteps; x++)
        {
            for (int i = 0; i < ropePartCount - 1; i++)
            {
                float mult = 1f / (ropePartRenderSteps + 1);

                renderInstances[i + x * ropePartCount].transform.position = GetBezierPosition(ropeParts[i].transform, ropeParts[i + 1].transform, (x + 1) * mult);

                //old method without bezier interpolation
                //renderInstances[i + x * ropePartCount].transform.position = Vector3.Lerp(ropeParts[i].transform.position, ropeParts[i+1].transform.position, (x+1) * mult);
            }
        }
    }

    void CreateRenderInstances()
    {
        renderInstances = new GameObject[ropePartCount * ropePartRenderSteps];
        for (int i = 0; i < renderInstances.Length; i++)
        {
            renderInstances[i] = Instantiate(ropePartRenderer);
            renderInstances[i].transform.localScale = ropeRenderScale;
            //print("Render Instances = " + renderInstances.Length);
        }
    }

    Vector3 GetBezierPosition(Transform start, Transform end, float t)
    {
        Vector3 p0 = start.position;
        Vector3 p1 = p0 + start.up * 0.2f;
        Vector3 p3 = end.position;
        Vector3 p2 = p3 - end.up * 0.2f;

        // here is where the magic happens!
        return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
    }
}
