using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpooderBossAI : MonoBehaviour
{
    private int lastAttackIndex = -1;
    [SerializeField] private float delayBetweenAttacks = 2.0f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();// the Animator is expected always to be on the same GameObject as your script
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true) // Keeps the boss continuously performing attacks
        {
            yield return new WaitForSeconds(delayBetweenAttacks); // Wait for the specified delay
            int attackIndex = GetRandomAttackIndex();
            PerformAttack(attackIndex);
            lastAttackIndex = attackIndex; // Update the last attack index
        }
    }

    private int GetRandomAttackIndex()
    {
        int index;
        do
        {
            index = Random.Range(0, 8); // Randomly pick an index for 8 different attacks
        }
        while (index == lastAttackIndex); // Ensure the new attack is not the same as the last one
        return index;
    }

    private void PerformAttack(int index)
    {
        switch (index)
        {
            case 0:
                Attack1();
                break;
            case 1:
                Attack2();
                break;
            case 2:
                Attack3();
                break;
            case 3:
                Attack4();
                break;
            case 4:
                Attack5();
                break;
            case 5:
                Attack6();
                break;
            case 6:
                Attack7();
                break;
            case 7:
                Attack8();
                break;
        }
    }

    private void Attack1()
    {
        anim.SetTrigger("Attack1");
        // Additional effects and damage logic here
    }

    // Define other attacks similarly...
    private void Attack2() { anim.SetTrigger("Attack2"); }
    private void Attack3() { anim.SetTrigger("Attack3"); }
    private void Attack4() { anim.SetTrigger("Attack4"); }
    private void Attack5() { anim.SetTrigger("Attack5"); }
    private void Attack6() { anim.SetTrigger("Attack6"); }
    private void Attack7() { anim.SetTrigger("Attack7"); }
    private void Attack8() { anim.SetTrigger("Attack8"); }
}
