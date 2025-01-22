using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu]
public class EnrageAbility : Ability
{

    private PlayerStats playerStats;
    float initialdamage;
    float addedModifier;

    // Start is called before the first frame update

    public override void Activate(GameObject parent)
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        Debug.Log("Activating Enrage");
        initialdamage = playerStats.damage.GetValue();
        addedModifier = initialdamage * 0.2f;
        playerStats.damage.AddModifier(addedModifier);
        AbilityHelper helper = parent.GetComponent<AbilityHelper>();
        helper.StartAbilityCoroutine(WaitAndExecute());

    }

    private IEnumerator WaitAndExecute()
    {  
        Debug.Log("Waiting for " + activeTime + " seconds...");
        yield return new WaitForSeconds(activeTime);
        Debug.Log(activeTime + " seconds have passed!");
        // Execute code after the wait
        playerStats.damage.RemoveModifier(addedModifier);
    }
}

    



