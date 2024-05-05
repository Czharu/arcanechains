using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpooderBossAI : MonoBehaviour
{
    private int lastAttackIndex = -1;
    [SerializeField] private float delayBetweenAttacks = 2.0f;
    [SerializeField] private GameObject laserPrefab;

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
                Debug.Log("SpooderAttack1");
                Attack1();
                break;
            case 1:
                Debug.Log("SpooderAttack2");
                Attack2();
                break;
            case 2:
                Debug.Log("SpooderAttack3");
                Attack3();
                break;
            case 3:
                Debug.Log("SpooderAttack4");
                Attack4();
                break;
            case 4:
                Debug.Log("SpooderAttack5");
                Attack5();
                break;
            case 5:
                Debug.Log("SpooderAttack6");
                Attack6();
                break;
            case 6:
                Debug.Log("SpooderAttack7");
                Attack7();
                break;
            case 7:
                Debug.Log("SpooderAttack8");
                Attack8();
                break;
        }
    }

    private void Attack1()
    {
        anim.SetTrigger("Attack1");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void ShootLaser()//Continuously spawning and destroying GameObjects (like lasers) can be performance-intensive. Consider implementing an object pooling system for the lasers if they are spawned frequently.
    {
        Vector3 position = transform.position; // Adjust the position based on where the eyes are, if needed
        Quaternion rotation = Quaternion.identity; // Adjust the rotation if needed

        // Instantiate the laser prefab at the boss's position with no rotation
        GameObject laserInstance = Instantiate(laserPrefab, position, rotation);

        // Set the parent of the laserInstance to this GameObject (the boss)
        laserInstance.transform.SetParent(transform);

        // Optionally, you might want to set the local position or rotation if needed
        laserInstance.transform.localPosition = new Vector3(0, 0, 0); // Adjust this if the laser needs to be offset
        laserInstance.transform.localRotation = Quaternion.identity; // Adjust if the laser should have a specific rotation relative to the boss
    }

    // Define other attacks similarly...
    private void Attack2()
    {
        anim.SetTrigger("Attack2");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack3()
    {
        anim.SetTrigger("Attack3");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack4()
    {
        anim.SetTrigger("Attack4");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack5()
    {
        anim.SetTrigger("Attack5");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack6()
    {
        anim.SetTrigger("Attack6");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack7()
    {
        anim.SetTrigger("Attack7");
        ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack8()
    {
        anim.SetTrigger("Attack8");
        ShootLaser();
        // Additional effects and damage logic here
    }
}
