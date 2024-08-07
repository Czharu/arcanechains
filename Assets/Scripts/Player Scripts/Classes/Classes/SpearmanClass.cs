using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanClass : BasicClass
{
    
    public CharacterStats characterStats;
    private Passive passive;
    private Active active;
    public SpearmanClass(CharacterStats playerStats){
        characterStats = playerStats;
        Debug.Log("Creating Passive...");
        passive = new Passive(0,0,0,0,0,5,10,10, characterStats);
    }

    public Passive GetPassive(){
        return passive;
    }

    public Active GetActive(){
        return active;
    }

    public override void ActivatePassive(){
        Debug.Log("Activating Passive...");
        passive.ActivatePassive();
    }

    public override void DeactivatePassive()
    {
        passive.DeactivatePassive();
    }

    public override bool CheckRequirements()
    {
        Debug.Log(characterStats.damage.GetValue());
        if(characterStats.damage.GetValue() > 89 && characterStats.swiftness.GetValue() > 15){
            return true;
        }
        else{
            return false;
        }
    }

}
