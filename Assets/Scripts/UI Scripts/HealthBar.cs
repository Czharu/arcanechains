using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetHealth(float health){
        slider.value = health;
        Debug.Log("Damage Set on Bar");
    }

    public void SetMaxHealth(float health){
        slider.maxValue = health;
    }
}
