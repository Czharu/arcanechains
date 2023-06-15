using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private float dirX = 0f;
    private int jumpStage = 0;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpHeight = 14f;

    private enum MovementState  { idle, running, jumping, falling }
    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        dirX = Input.GetAxisRaw("Horizontal");
        if(Input.GetButton("Horizontal"))
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        if(Input.GetButtonDown("Jump") && JumpCheck())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState(){
        MovementState state;
        if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f && IsGrounded() == false)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f && IsGrounded() == false)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool JumpCheck()
    {
        if(IsGrounded())
        {
            jumpStage = 1;
            return true;
        }
        else if(jumpStage == 1)
            {
                jumpStage++;
                return true;
            }
        else
        {
            return false;
        }
        
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, jumpableGround);
    }
}
