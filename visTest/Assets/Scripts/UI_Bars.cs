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

    public bool charging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cur_value = max_value;
        //InvokeRepeating("increasePowerUp", 0f, 2f); //name, time, repeatrate
    }

    private void Update()
    {
        if(cur_value != max_value)
        {
            charging = true;
        }

        if (Input.GetKeyDown(mechanic) && !charging)
        {
            cur_value = 0f;

         
            InvokeRepeating("increasePowerUp", 0f, 2f); //name, time, repeatrate
           
        }
    }
    //If player uses grenade, the image will go to value 0 and one number will dissapear

    //Image will start filling after
    void increasePowerUp()
    {
        //cur_value += fillSpeed;
        float calc_value = cur_value / max_value; //70/100 = 0.7 = %
        SetValue(calc_value);
    }

    //If the bar is filled, a new grenade will be usable



    void SetValue(float myvalue)
    {
        Bar.fillAmount = myvalue;
    }
}
