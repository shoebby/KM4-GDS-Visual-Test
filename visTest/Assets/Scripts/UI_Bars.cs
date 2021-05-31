using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bars : MonoBehaviour
{
    public Image Bar;

    public float max_value = 100f;
    public float cur_value = 0f;
    public float fillSpeed;

    public KeyCode mechanic;

    public int max_grenades;
    public int grenades_left; //Later matchen met daadwerkelijk limiet
    public int grenades_gifted;
    public Text grenades_Left_Text;

    public bool charging = false;
    public bool showGrenadeCount = false;

    
  
    void Start()
    {
        grenades_Left_Text.gameObject.SetActive(showGrenadeCount);
        cur_value = max_value;
        grenades_left = max_grenades;
    }

    private void Update()
    {
        grenades_Left_Text.text = grenades_left.ToString();
        
        //If player uses grenade, the image will go to value 0 and one number will dissapear
        /* Moved this functionality to the player controller so we can cancel throwing a grenade
        if (Input.GetKeyDown(mechanic) && grenades_left >= 1)
        {
            grenades_left -= grenades_gifted;
            StartCharging();
        }
        */

        if (cur_value < max_value)
        {
            charging = true;
        } else if (cur_value >= max_value)
        {
            charging = false;
        }

        if (!charging && grenades_left < max_grenades)
        {
            grenades_left += grenades_gifted;
        }

        //Debug.Log(grenades_left);
        //Debug.Log(charging);
    }

    public void StartCharging()
    {
        if (charging) { return; }
        cur_value = 0f;
        InvokeRepeating("IncreasePowerUp", 0f, .5f); //name, time, repeatrate
    }

    //Image will start filling after
    void IncreasePowerUp()
    {
        cur_value += fillSpeed;
        Bar.fillAmount = (float)cur_value / (float)max_value;
        //Debug.Log(Bar.fillAmount);
    }

    //If the bar is filled, a new grenade will be usable
}
