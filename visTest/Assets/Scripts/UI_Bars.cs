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

    
  
    void Start()
    {
        cur_value = max_value;
        grenades_left = max_grenades;
    }

    private void Update()
    {
        grenades_Left_Text.text = grenades_left.ToString();
           
        if(cur_value != max_value)
        {
            charging = true;
        }

        //If player uses grenade, the image will go to value 0 and one number will dissapear
        if (Input.GetKeyDown(mechanic) && !charging)
        {
            grenades_left = grenades_left - grenades_gifted;
            cur_value = 0f;

            InvokeRepeating("increasePowerUp", 0f, .5f); //name, time, repeatrate

        }

        if(!charging && grenades_left != max_grenades)
        {
            grenades_left = grenades_left + grenades_gifted;
        }
        
        Debug.Log(grenades_left);
        Debug.Log(charging);
    }

    //Image will start filling after
    void increasePowerUp()
    {
        cur_value += fillSpeed;
        Bar.fillAmount = (float)cur_value / (float)max_value;
    }

    //If the bar is filled, a new grenade will be usable
}
