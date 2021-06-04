using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreCalculation : MonoBehaviour
{
    public Image Bar;

    public float max_value = 100f;
    public float cur_value = 0f;
    public float fillSpeed;

    public float scoreCount = 0; //Hier de score van de speler berekenen

    public bool charging = false;


    void Start()
    {
        StartCharging();
    }

    private void Update()
    {
        if (cur_value < scoreCount)
        {
            charging = true;
        }
        else
        {
            charging = false;
        }
    }

    public void StartCharging()
    {
        if (charging) { return; }
        //cur_value = 0f;
        InvokeRepeating("IncreasePowerUp", 0f, .5f); //name, time, repeatrate
    }

    //Image will start filling after
    void IncreasePowerUp()
    {
        cur_value += fillSpeed;
        Bar.fillAmount = (float)cur_value / (float)max_value;
        //Debug.Log(Bar.fillAmount);
    }
}
