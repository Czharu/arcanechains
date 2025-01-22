using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Animations;

public class EnemyAIFlying : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    int currentWaypoint = 0;
#pragma warning disable
    bool reachedEndOfPath = false;
#pragma warning restore
    private Seeker seeker;
    private Rigidbody2D rb;
    public Transform enemyGFX;
    private Animator anim; // Reference to the Animator component
    public bool chasingRight = false; //this is used as a debug. if the enemy's initial landing doesn't flip set this.
    public int rangedAttackCooldown = 0; // Cooldown for ranged attack

    // Ranged attack settings
    [SerializeField] private bool enableRangedAttack = false; // Enable/Disable ranged attack
    [SerializeField] private float RangedAttackAnimDelay = 0.5f; //Delay for a first ranged animation to play before ranged attack formation
    [SerializeField] private int rangedAttackDistanceMin = 3; // Minimum distance for ranged attack
    [SerializeField] private int rangedAttackDistanceMax = 17; // Maximum distance for ranged attack
    [SerializeField] private GameObject projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform projectileSpawnPoint; // Position where the projectile will be spawned
    [SerializeField] private int numberOfProjectiles = 1; // Number of projectiles to be spawned
    [SerializeField] private float timeBetweenProjectiles = 0.5f; // Time between each projectile spawn
    [SerializeField] private int RangedAttackCD = 200;
    private AttackHandler attackHandler; // attached script to manage the attacks
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 1f);
        attackHandler = GetComponent<AttackHandler>(); // Get the AttackHandler component
        anim = GetComponent<Animator>(); // Initialize the Animator component

    }
    private bool HasParameter(string paramName, Animator animator)// to get rid of animator parameter caution events
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
    // Method to calculate cooldown with RNG variability
    private int CalculateCooldown(int baseCooldown)
    {
        float rngFactor = Random.Range(0.95f, 1.35f); // ±% variability to attack cooldowns
        return Mathf.RoundToInt(baseCooldown * rngFactor);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            chasingRight = true;
            AdjustProjectileScale(); // Adjust the scale of any attached projectiles
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            chasingRight = false;
            AdjustProjectileScale(); // Adjust the scale of any attached projectiles
        }
        

        //ranged attack
        float distanceFromPlayer = target.position.x - transform.position.x;

        if (enableRangedAttack && Mathf.Abs(distanceFromPlayer) >= rangedAttackDistanceMin && Mathf.Abs(distanceFromPlayer) <= rangedAttackDistanceMax && rangedAttackCooldown == 0) // Ranged attack when within range and enabled
        {
            rangedAttackCooldown = CalculateCooldown(RangedAttackCD); // Cooldown duration for the next ranged attack

            if (HasParameter("RangedAttack", anim))
            {
                anim.SetTrigger("RangedAttack"); // Trigger the initial ranged attack animation
            }

            StartCoroutine(RangedAttackCoroutine(RangedAttackAnimDelay));

        }
        else
        {
            if (rangedAttackCooldown > 0) rangedAttackCooldown--;
        }


    }


    private IEnumerator RangedAttackCoroutine(float delay)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            yield return new WaitForSeconds(delay);
            attackHandler?.RangedAttack(projectilePrefab, projectileSpawnPoint, chasingRight);
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
        if (HasParameter("RangedAttackFinished", anim))
        {
            anim.SetTrigger("RangedAttackFinished"); // Trigger the return back to normal
        }
    }
    private void AdjustProjectileScale()
    {
        // Find any attached projectiles and adjust their scale
        Projectile[] attachedProjectiles = GetComponentsInChildren<Projectile>();
        foreach (Projectile projectile in attachedProjectiles)
        {
            Vector3 scale = projectile.transform.localScale;
            scale.x = enemyGFX.localScale.x*-1; // Ensure x-scale is positive
            projectile.transform.localScale = scale;
        }
    }
}
