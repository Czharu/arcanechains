using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicClass
{
    private Passive passive;
    private Active active;
    Passive GetPassive(){
        return passive;
    }
    Active GetActive(){
        return active;
    }

    public abstract void ActivatePassive();
    public abstract void DeactivatePassive();

    public abstract bool CheckRequirements();
}
