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
    public GameObject[] defensiveOrbs; // Assign this via the Inspector or fetch dynamically for Compass45()

     // Define delays for each attack
    private float[] attackDelays = new float[] { 1f, 13f, 13f, 7f, 4f, 2f, 1f, 1f };

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
            yield return new WaitForSeconds(attackDelays[attackIndex]); // Wait for the specified delay after each attack
            Debug.Log(attackDelays[attackIndex]);
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
        anim.SetTrigger("Attack4");

        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack5()
    {
        anim.SetTrigger("Attack5");

        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack6()
    {
        anim.SetTrigger("Attack1");

        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack7()
    {
        anim.SetTrigger("Attack1");
        //ShootLaser();
        // Additional effects and damage logic here
    }
    private void Attack8()
    {
        anim.SetTrigger("Attack1");
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
                // Initialize aiming at the player
                laserComponent.Initialize(player.transform.position, 5f);  // Fast laser
            }
        }
    }

    public void ShootStarLaser()
    {
        starLaserPrefab.transform.position = StarLazerOriginPoint.transform.position; // Move the existing object to the origin point

        leftlegsAnimator.SetTrigger("LeftLegAttack2");
        starLaserPrefab.SetActive(true);  // Activate the existing object  // Ensure the prefab is active
        // Find the StarWarning child and enable its collider after 1 second
        StarBeam starBeam = starLaserPrefab.GetComponentInChildren<StarBeam>();
        if (starBeam != null)
        {
            starBeam.EnableColliderAfterDelay(2f);
        }
        StartCoroutine(DisableAfterDelay(starLaserPrefab, 11.5f));  // Disable the object after 10 seconds
    }
    public void ShootStarLaserReverse()
    {
        starLaserPrefabReverse.transform.position = StarLazerOriginPoint.transform.position; // Move the existing object to the origin point

        rightlegsAnimator.SetTrigger("RightLegsAttack3");
        starLaserPrefabReverse.SetActive(true);  // Activate the existing object  // Ensure the prefab is active
        // Find the StarWarning child and enable its collider after 1 second
        StarBeam starBeam = starLaserPrefabReverse.GetComponentInChildren<StarBeam>();
        if (starBeam != null)
        {
            starBeam.EnableColliderAfterDelay(2f);
        }
        StartCoroutine(DisableAfterDelay(starLaserPrefabReverse, 11.5f));  // Disable the object after 10 seconds
    }
    // Define the directions for Compass45
    private Vector3[] compass45Directions = new Vector3[]
    {
        new Vector3(-1, -1, 0).normalized,
        new Vector3(1, 1, 0).normalized,
        new Vector3(-1, 1, 0).normalized,
        new Vector3(1, -1, 0).normalized
    };

    public void Compass45()
    {
        StartCoroutine(SpawnLasersWithDelay());
    }
    private IEnumerator SpawnLasersWithDelay()
    {
        // Determine the direction of iteration
        bool reverse = Random.value > 0.5;
        // Create a temporary list of orbs to potentially reverse the order
        List<GameObject> orbs = new List<GameObject>(defensiveOrbs);
        if (reverse)
        {
            orbs.Reverse();
        }
        foreach (GameObject orb in orbs)
        {
            yield return new WaitForSeconds(0.3f);  // Wait for 0.1 seconds before spawning the next laser
            foreach (Vector3 dir in compass45Directions)
            {
                SpawnLaserAtOrb(orb, dir, 1.5f);  // Spawn laser

            }
        }
    }

    void SpawnLaserAtOrb(GameObject orb, Vector3 direction, float speed)
    {
        Vector3 spawnPosition = orb.transform.position; // Get the position of the orb
        GameObject laserInstance = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
        Laser laserComponent = laserInstance.GetComponent<Laser>();

        if (laserComponent != null)
        {
            // Calculate the direction vector for 45 degrees downward and to the left
            //Vector3 direction = new Vector3(-1, -1, 0).normalized;
            laserComponent.Initialize(direction, speed, true); // Set the direction without targeting the player
            laserComponent.DisableAnimator(); // Disable the animator if it exists
        }
    }

    private IEnumerator DisableAfterDelay(GameObject obj, float delay)// disables game object after x seconds
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void OutShoot()
    {
        StartCoroutine(OutShootSequence());
    }

    private IEnumerator OutShootSequence()
    {
        List<GameObject> lasers = new List<GameObject>();



        // Instantiate lasers at each orb and store them in a list.
        foreach (GameObject orb in defensiveOrbs)
        {
            Vector3 spawnPosition = orb.transform.position;
            GameObject laserInstance = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
            Laser laserComponent = laserInstance.GetComponent<Laser>();
            if (laserComponent != null)
            {
                // Calculate direction from the boss to the orb
                Vector3 directionFromBossToOrb = (orb.transform.position - transform.position).normalized;

                laserComponent.Initialize(directionFromBossToOrb, 0f, true); // Initialize with zero speed.
                //Debug.Log(directionFromBossToOrb);
            }
            lasers.Add(laserInstance);
        }

        // Wait for 3 seconds before firing them.
        yield return new WaitForSeconds(3);

        // Set each laser to move in the opposite direction to the boss with the intended speed.
        foreach (GameObject laser in lasers)
        {
            Laser laserComponent = laser.GetComponent<Laser>();
            if (laserComponent != null)
            {
                laserComponent.SetSpeed(3f); // Set the speed after 3 seconds.
            }
        }
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

