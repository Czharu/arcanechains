using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnrageAbilityBehaviour : MonoBehaviour
{
    private PlayerStats playerStats;
    float initialattackspeed;
    float addedModifier;
    // Start is called before the first frame update
    public void startAbility(float activeTime){
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        initialattackspeed = playerStats.attackSpeed.GetValue();
        addedModifier = initialattackspeed * 0.25f;
        playerStats.attackSpeed.AddModifier(addedModifier);
        StartCoroutine(WaitAndExecute(activeTime));
    }

    IEnumerator WaitAndExecute(float activeTime)
    {
        Debug.Log("Waiting for 5 seconds...");
        
        // Wait for 5 seconds
        yield return new WaitForSeconds(activeTime);

        Debug.Log("5 seconds have passed!");
        
        // Execute code after the wait
        playerStats.attackSpeed.RemoveModifier(addedModifier);
        
    }
}
