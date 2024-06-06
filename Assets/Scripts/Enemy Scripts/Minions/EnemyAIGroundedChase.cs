using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the enemy
    public Transform playerTransform; // Reference to the player's transform
    private bool isChasing;

    public bool chasingRight = true;
    public float chaseDistance = 7f; // Distance at which the enemy starts chasing
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim; // Reference to the Animator component
    public int jumpCooldown = 0;
    public int standAttackCooldown = 0;

    //attack cchangeable values and toggles
    [SerializeField] private bool enableJumpAttack = true; // Enable/Disable jump attack
    [SerializeField] private int attackDistance = 2; // Distance at which the enemy performs jump attack
    [SerializeField] private int jumpHeight = 2; // Height of the jump
    [SerializeField] private float jumpStrength = 5f; // Strength of the horizontal jump    
    [SerializeField] private bool enableStandAttack = true; // Enable/Disable standing attack
    [SerializeField] private int standAttackDistance = 2; // Distance at which the enemy performs standing attack
    //other chaneable values    
    [SerializeField] private float flipOffset = 2f; // Offset to prevent flipping when directly below the player
    [SerializeField] private LayerMask jumpableGround; // Layer mask to check if the enemy is grounded
    private bool isColliding = false;
    private AttackHandler attackHandler; // attached script to manage the attacks
    // Reference to the child GameObject with animations
    private Transform enemyChild;
    private BoxCollider2D childCollider; // Reference to the child's BoxCollider2D
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();        
        anim = GetComponentInChildren<Animator>(); // Initialize the Animator component
        attackHandler = GetComponentInChildren<AttackHandler>(); // Get the AttackHandler component
        enemyChild = transform.GetChild(0); // Get the child GameObject (assuming it's the first child)
        childCollider = enemyChild.GetComponent<BoxCollider2D>(); // Get the child's BoxCollider2D
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
                    if (distanceFromPlayer < -flipOffset && chasingRight)
                    {
                        chasingRight = false;
                        Flip();
                    }
                    else if (distanceFromPlayer > flipOffset && !chasingRight)
                    {
                        chasingRight = true;
                        Flip();
                    }
                    rb.velocity = new Vector2(chasingRight ? moveSpeed : -moveSpeed, rb.velocity.y);// simplified movement direction code
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
                        jumpCooldown = 100; // Cooldown duration for the next jump attack
                    }
                }
                // Standing attack when close enough to the player and stand attack is enabled
                if (isChasing && enableStandAttack && Mathf.Abs(distanceFromPlayer) < standAttackDistance) 
                {
                    isChasing = false;
                    attackHandler?.StandAttack();
                    standAttackCooldown = 100; // Cooldown duration for the next standing attack
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
                anim.SetBool("Chase", true);
            }
            else
            {
                anim.SetBool("Chase", false);
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
    {
        if (IsGrounded())
        {
            Vector3 localScale = enemyChild.localScale;
            localScale.x = chasingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            enemyChild.localScale = localScale;
        }
    }
}
