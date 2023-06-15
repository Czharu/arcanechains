using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetHealth(int health){
        slider.value = health;
        Debug.Log("Damage Set on Bar");
    }

    public void SetMaxHealth(int health){
        slider.maxValue = health;
    }
}
