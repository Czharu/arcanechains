using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script to attach to attack state in the Animator to ensure that once the state completes, the transition back to "Idle" is triggered
//This script is no longer used, exit time seems better.
public class ResetIdleStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3") ||stateInfo.IsName("Attack4") ||stateInfo.IsName("Attack5") ||stateInfo.IsName("Attack6") ||stateInfo.IsName("Attack7") ||stateInfo.IsName("Attack8")) // check for other attack states similarly
        {
            animator.SetTrigger("returnToIdle");
        }
    }
}
