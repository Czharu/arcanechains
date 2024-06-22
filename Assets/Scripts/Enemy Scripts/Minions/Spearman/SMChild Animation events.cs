using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimatorEvents : MonoBehaviour
//This is a script purely to allow a child animation clip to call methods from a parent AttackHandler script
{
    // Reference to the parent AttackHandler script
    private AttackHandler parentAttackHandler;

    void Start()
    {
        // Get the reference to the parent AttackHandler script
        parentAttackHandler = GetComponentInParent<AttackHandler>();
    }

    // Method to be called by the animation event
    public void CallParentAnimStandAttack()
    {
        if (parentAttackHandler != null)
        {
            parentAttackHandler.AnimStandAttack();
            //Debug.Log("connect");
        }
    }
}