using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBoxBehaviour : MonoBehaviour
{

    public GameObject[] chargedIndicators;
    [HideInInspector] public Light boxLight;
    [HideInInspector] public Battery battery;
    [HideInInspector] public ContactBox box;

    void Start()
    {
        foreach (GameObject g in chargedIndicators)
        {
            g.SetActive(false);
        }

        boxLight = GetComponentInChildren<Light>();
        boxLight.gameObject.SetActive(false);
        battery = GetComponentInChildren<Battery>();
        box = GetComponentInChildren<ContactBox>();
    }

    private void Update()
    {
        if(battery.charges > 0)
        {
            boxLight.gameObject.SetActive(true);
        }
        else
        {
            boxLight.gameObject.SetActive(false);
        }
        
    }

    public void UpdateIndicators()
    {
        for (int i = 0; i < chargedIndicators.Length; i++)
        {
            chargedIndicators[i].SetActive(false);
        }

        for (int i = 0; i < battery.charges; i++)
        {
            chargedIndicators[i].SetActive(true);
        }
    }
}
