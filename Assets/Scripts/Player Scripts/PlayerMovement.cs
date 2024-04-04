using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;// include the appropriate namespace for the Tilemap classes

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private int jumpStage = 0;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpHeight = 14f;


    private enum MovementState { idle, running, jumping, falling }
    [SerializeField] private AudioSource jumpSoundEffect;
    //Controls input references
    [SerializeField] private InputActionReference attack;
    [SerializeField] private RotateWeaponOnClick weaponParent;
    private float horizontal;

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
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        faceMouse2();
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (horizontal > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && IsGrounded() == false)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f && IsGrounded() == false)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool JumpCheck()
    {
        if (IsGrounded())
        {
            jumpStage = 1;
            return true;
        }
        else if (jumpStage == 1)
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

    //Calls Player Input/Events components
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && JumpCheck())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpSoundEffect.Play();
        }

        // Slows jump if button released
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }


    //Getting the player to face the mouse
    private void faceMouse2()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - sprite.transform.position.x,
            mousePosition.y - sprite.transform.position.y
        );


        if (direction.x < 0)
        {
            sprite.flipX = true;
        }
        else if (direction.x > 0)
        {
            sprite.flipX = false;
        }

    }

    //perform attack
    private void OnEnable()
    {
        attack.action.performed += PerformAttack;

    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;


    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weaponParent.Attack();
    }
}
