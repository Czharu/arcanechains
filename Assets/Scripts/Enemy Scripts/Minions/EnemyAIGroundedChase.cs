using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the enemy
    public Transform playerTransform; // Reference to the player's transform
    private bool isChasing;

    public bool chasingRight = true; //this is used as a debug. if the enemy's initial landing doesn't flip set this.
    public float chaseDistance = 7f; // Distance at which the enemy starts chasing
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim; // Reference to the Animator component
    public int jumpCooldown = 0;
    public int standAttackCooldown = 0;
    public int rangedAttackCooldown = 0;

    //attack cchangeable values and toggles
    [SerializeField] private bool enableJumpAttack = true; // Enable/Disable jump attack
    [SerializeField] private int attackDistance = 2; // Distance at which the enemy performs jump attack
    [SerializeField] private int jumpHeight = 2; // Height of the jump
    [SerializeField] private float jumpStrength = 5f; // Strength of the horizontal jump    
    [SerializeField] private bool enableStandAttack = true; // Enable/Disable standing attack
    [SerializeField] private float standAttackDistance = 2; // Distance at which the enemy performs standing attack
    //Stand attak also is the minimum distane that chase is deactivated at
    // Ranged attack settings
    [SerializeField] private bool enableRangedAttack = true; // Enable/Disable ranged attack
    [SerializeField] private float RangedAttackAnimDelay = 0.5f; //Delay for a first ranged animation to play before ranged attack formation
    [SerializeField] private int rangedAttackDistanceMin = 3; // Minimum distance for ranged attack
    [SerializeField] private int rangedAttackDistanceMax = 17; // Maximum distance for ranged attack
    [SerializeField] private GameObject projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform projectileSpawnPoint; // Position where the projectile will be spawned
    [SerializeField] private int numberOfProjectiles = 1; // Number of projectiles to be spawned
    [SerializeField] private float timeBetweenProjectiles = 0.5f; // Time between each projectile spawn

    //other chaneable values    
    [SerializeField] private float flipOffset = 2f; // Offset to prevent flipping when directly below the player
    [SerializeField] private LayerMask jumpableGround; // Layer mask to check if the enemy is grounded
    private bool isColliding = false;
    private AttackHandler attackHandler; // attached script to manage the attacks
    // Reference to the child GameObject with animations
    private Transform enemyChild;
    private BoxCollider2D childCollider; // Reference to the child's BoxCollider2D
    // Start is called before the first frame update
    [SerializeField] private bool IsRoller = false; // Enable/Disable flip
    [SerializeField] private int RangedAttackCD = 200;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>(); // Initialize the Animator component
        attackHandler = GetComponentInChildren<AttackHandler>(); // Get the AttackHandler component
        enemyChild = transform.GetChild(0); // Get the child GameObject (assuming it's the first child)
        childCollider = enemyChild.GetComponent<BoxCollider2D>(); // Get the child's BoxCollider2D        
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

    // Update is called once per frame
    private void FixedUpdate()
    {
        {
            float distanceFromPlayer = playerTransform.position.x - enemyChild.position.x;
            if (!isColliding && jumpCooldown == 0 && standAttackCooldown == 0)
            {
                if (isChasing) //revised code such that it calculates if the enemy should be flipped
                {
                    if (IsGrounded()) // Ensure enemy only changes direction when grounded
                    {
                        if (distanceFromPlayer < -flipOffset && chasingRight)
                        {
                            chasingRight = false;
                            if (!IsRoller)
                            {
                                Flip();
                            }

                        }
                        else if (distanceFromPlayer > flipOffset && !chasingRight)
                        {
                            chasingRight = true;
                            if (!IsRoller)
                            {
                                Flip();
                            }

                        }
                    }
                    if (IsRoller)
                    {
                        Roll();
                    }
                    else if (IsGrounded())
                    {
                        rb.velocity = new Vector2(chasingRight ? moveSpeed : -moveSpeed, rb.velocity.y);// simplified movement direction code
                    }
                }

                else
                {
                    if (Vector2.Distance(enemyChild.position, playerTransform.position) < chaseDistance)
                    {
                        isChasing = true;
                    }
                }
                if (isChasing && enableJumpAttack) // Jump attack when close enough to the player and jump attack is enabled
                { // Jump attack when close enough to the player
                    if (Mathf.Abs(distanceFromPlayer) < attackDistance)
                    {
                        isChasing = false;
                        attackHandler?.JumpAttack(playerTransform.position, jumpHeight, jumpStrength);
                        jumpCooldown = CalculateCooldown(100); // Cooldown duration for the next jump attack
                    }
                }
                // Standing attack when close enough to the player and stand attack is enabled
                if (isChasing && enableStandAttack && Mathf.Abs(Vector2.Distance(enemyChild.position, playerTransform.position)) < standAttackDistance)
                {
                    isChasing = false;
                    attackHandler?.StandAttack();
                    jumpCooldown = CalculateCooldown(100); // Cooldown duration for the next standing attack
                }
                //removed chase requirement for ranged attack
                if (enableRangedAttack && Mathf.Abs(distanceFromPlayer) >= rangedAttackDistanceMin && Mathf.Abs(distanceFromPlayer) <= rangedAttackDistanceMax && rangedAttackCooldown == 0) // Ranged attack when within range and enabled
                {
                    rangedAttackCooldown = CalculateCooldown(RangedAttackCD); // Cooldown duration for the next ranged attack
                    isChasing = false;
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
            else //recover cooldowns
            {
                if (jumpCooldown > 0) jumpCooldown--;
                if (standAttackCooldown > 0) standAttackCooldown--;

            }
            // Set the animation state
            if (isChasing)
            {
                if (HasParameter("Chase", anim))
                {
                    anim.SetBool("Chase", true);
                }
            }
            else
            {
                if (HasParameter("Chase", anim))
                {
                    anim.SetBool("Chase", true);
                }
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(childCollider.bounds.center, childCollider.bounds.size, 0f, Vector2.down, .2f, jumpableGround);
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = true;
            rb.velocity.Set(0, 0); // Stop moving when colliding with the player

        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
    void Flip()// flips the direction the enemy is facing when grounded based on side of the player it is
    //slight issue, a small hop prevents flipping when the interaction is meant to keep the enemy moving same direction
    {
        if (IsGrounded())
        {
            Vector3 localScale = enemyChild.localScale;
            localScale.x = chasingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            enemyChild.localScale = localScale;
        }
    }
    void Roll()
    {
        if (IsGrounded())
        {
            float targetVelocity = chasingRight ? moveSpeed : -moveSpeed;
            rb.velocity = new Vector2(targetVelocity, rb.velocity.y);
            rb.angularVelocity = -targetVelocity * 100; // Adjust multiplier as needed for smooth rolling
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
}
