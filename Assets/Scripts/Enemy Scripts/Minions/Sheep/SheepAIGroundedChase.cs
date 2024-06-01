using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 0.2f; // Speed of the enemy
    public Transform playerTransform; // Reference to the player's transform
    private bool isChasing;
    public bool chasingRight = false;
    public float chaseDistance = 7; // Distance at which the enemy starts chasing
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public int jumpCooldown = 0;

    [SerializeField] int jumpHeight = 2; // Height of the jump
    [SerializeField] private float jumpStrength = 5f; // Strength of the horizontal jump

    [SerializeField] int attackDistance = 2; // Distance at which the enemy attacks

    [SerializeField] private LayerMask jumpableGround; // Layer mask to check if the enemy is grounded
    bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        {
            float distanceFromPlayer = playerTransform.position.x - transform.position.x;
            if (!isColliding && jumpCooldown == 0)
            {
                if (isChasing)
                {
                    if (transform.position.x > playerTransform.position.x)
                    {
                        //transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                        rb.velocity = new Vector2(moveSpeed * -1, rb.velocity.y);
                        chasingRight = false;
                    }
                    if (transform.position.x < playerTransform.position.x)
                    {
                        //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                        chasingRight = true;

                    }
                }

                else
                {
                    if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                    {
                        isChasing = true;
                    }
                }
                if (isChasing)
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
}
