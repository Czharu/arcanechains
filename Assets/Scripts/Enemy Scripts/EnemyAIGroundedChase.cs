using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public Transform playerTransform;
    private bool isChasing;
    public bool chasingRight = false;
    public float chaseDistance = 7;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public int jumpCooldown = 0;

    [SerializeField] int jumpHeight = 2;

    [SerializeField] int attackDistance = 2;

    [SerializeField] private LayerMask jumpableGround;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
    {
        float distanceFromPlayer = playerTransform.position.x - transform.position.x;
        if(isChasing){
            if(transform.position.x > playerTransform.position.x){
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                chasingRight = false;
                if(jumpCooldown > 0)
                {
                jumpCooldown --;
                }
            }
            if(transform.position.x < playerTransform.position.x){
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                chasingRight = true;
                if(jumpCooldown > 0){
                jumpCooldown--;
                }
            
        }
        }

        else {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance){
                isChasing = true;
            }
        }
        if(isChasing && jumpCooldown == 0){
            if(Mathf.Abs(distanceFromPlayer) < attackDistance) {
            isChasing = false;
            JumpAttack();
            jumpCooldown = 100;
            }
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
            rb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
    }
}
