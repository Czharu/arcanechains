using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 0.2f; // Speed of the enemy
    public Transform playerTransform; // Reference to the player's transform
    private bool isChasing;

    public bool chasingRight = true;
    public float chaseDistance = 7; // Distance at which the enemy starts chasing
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim; // Reference to the Animator component
    public int jumpCooldown = 0;

    [SerializeField] private int jumpHeight = 2; // Height of the jump
    [SerializeField] private float jumpStrength = 5f; // Strength of the horizontal jump

    [SerializeField] private int attackDistance = 2; // Distance at which the enemy attacks

    [SerializeField] private LayerMask jumpableGround; // Layer mask to check if the enemy is grounded
    [SerializeField] private bool enableJumpAttack = true; // Enable/Disable jump attack
    [SerializeField] private float flipOffset = 0.5f; // Offset to prevent flipping when directly below the player
    private bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); // Initialize the Animator component
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        {
            float distanceFromPlayer = playerTransform.position.x - transform.position.x;
            if (!isColliding && jumpCooldown == 0)
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
                    if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                    {
                        isChasing = true;
                    }
                }
                if (isChasing && enableJumpAttack) // Jump attack when close enough to the player and jump attack is enabled
                { // Jump attack when close enough to the player
                    if (Mathf.Abs(distanceFromPlayer) < attackDistance)
                    {
                        isChasing = false;
                        JumpAttack();
                        jumpCooldown = 100; // Cooldown duration for the next jump attack
                    }
                }
            }
            else if (jumpCooldown > 0)
            {
                jumpCooldown--;
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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, jumpableGround);
    }

    void JumpAttack()
    {
        float distanceFromPlayer = playerTransform.position.x - transform.position.x;

        if (IsGrounded())
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Reset horizontal velocity
            float horizontalForce = distanceFromPlayer > 0 ? jumpStrength : -jumpStrength; // Determine the direction of the horizontal force
            rb.AddForce(new Vector2(horizontalForce, jumpHeight), ForceMode2D.Impulse); // Apply the jump force
            // Trigger the JumpAttack animation
            anim.SetTrigger("JumpAttack");
        }
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
            Vector3 localScale = transform.localScale;
            localScale.x = chasingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }
}
