using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpooderBossAI : MonoBehaviour
{
    private int lastAttackIndex = -1;

    [SerializeField] private GameObject laserPrefab;
    public GameObject starLaserPrefab;
    public GameObject starLaserPrefabReverse;
    public GameObject StarLazerOriginPoint;
    public GameObject MouthPosition;

    private Animator anim;
    // animators
    public Animator headAnimator;
    public Animator leftlegsAnimator;
    public Animator rightlegsAnimator;

    private void Start()
    {
        anim = GetComponent<Animator>();// the Animator is expected always to be on the same GameObject as your script
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true) // Keeps the boss continuously performing attacks
        {
            
            int attackIndex = GetRandomAttackIndex();
            PerformAttack(attackIndex);
            // Specific delay after Attack2
            if (attackIndex >= 1)  // Assuming index 1 corresponds to Attack2
            {
                yield return new WaitForSeconds(11);  // Additional 10 second wait after Attack2
            }
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
        // ShootLaser();
        // Additional effects and damage logic here
    }


    // Define other attacks similarly...
    private void Attack2()
    {
        anim.SetTrigger("Attack2");
        
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack3()
    {
        anim.SetTrigger("Attack3");
        
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack4()
    {
        anim.SetTrigger("Attack3");
        
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack5()
    {
        anim.SetTrigger("Attack3");
        
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack6()
    {
        anim.SetTrigger("Attack3");
        
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack7()
    {
        anim.SetTrigger("Attack7");
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack8()
    {
        anim.SetTrigger("Attack8");
        //ShootLaser();
        // Additional effects and damage logic here
    }
    //Attacks Called through Animation Clip  Events
    public void ShootLaser()//Continuously spawning and destroying GameObjects (like lasers) can be performance-intensive. Consider implementing an object pooling system for the lasers if they are spawned frequently.
    {
        Vector3 position = MouthPosition.transform.position;  // Use the eye position GameObject
        GameObject laserInstance = Instantiate(laserPrefab, position, Quaternion.identity);
        //laserInstance.transform.SetParent(transform); // Optionally, you might not need this if you want the laser to move independently of the boss

        //Assuming you have a way to find the player, e.g., tagging the player with "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Laser laserComponent = laserInstance.GetComponent<Laser>();
            if (laserComponent != null)
            {
                laserComponent.Initialize(player.transform.position);
            }
        }
    }

    public void ShootStarLaser()
    {
        starLaserPrefab.transform.position = StarLazerOriginPoint.transform.position; // Move the existing object to the origin point
        
        leftlegsAnimator.SetTrigger("LeftLegAttack2");
        starLaserPrefab.SetActive(true);  // Activate the existing object  // Ensure the prefab is active

        StartCoroutine(DisableAfterDelay(starLaserPrefab, 9.5f));  // Disable the object after 10 seconds
    }
    public void ShootStarLaserReverse()
    {
        starLaserPrefabReverse.transform.position = StarLazerOriginPoint.transform.position; // Move the existing object to the origin point
        
        rightlegsAnimator.SetTrigger("RightLegsAttack3");
        starLaserPrefabReverse.SetActive(true);  // Activate the existing object  // Ensure the prefab is active

        StartCoroutine(DisableAfterDelay(starLaserPrefabReverse, 9.5f));  // Disable the object after 10 seconds
    }

    private IEnumerator DisableAfterDelay(GameObject obj, float delay)// disables game object after x seconds
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }


    //Child Animator Triggers for animatons on Animator
    public void PlayHeadChange()
    {
        headAnimator.SetTrigger("HeadChange");
    }

    public void PlayLeftLegsSwing()
    {
        leftlegsAnimator.SetTrigger("LeftLegsSwing");
    }

    public void PlayRightLegsSwing()
    {
        rightlegsAnimator.SetTrigger("RightLegsSwing");
    }
}

