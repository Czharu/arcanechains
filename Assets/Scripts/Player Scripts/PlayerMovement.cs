using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;// include the appropriate namespace for the Tilemap classes
//Player Controller Script
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

    private List<Follower> followers = new List<Follower>();//followers

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

    public void SetPosition(Vector3 newPosition) // public method to set the player's position
    {
        transform.position = newPosition;
        rb.velocity = Vector2.zero; // Resetting velocity to avoid continuous movement after teleporting
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
        if (IsGrounded() || coyoteTimeCounter > 0) // Allows jumping within coyote time
        {
            jumpStage = 1; // Reset jumps when grounded or in coyote time
            return true;
        }
        else if (jumpStage == 0) // Player fell off a ledge without jumping yet
        {
            jumpStage = 1; // Assign them their first jump in the air so they have air jump
            return true;
        }
        else if (jumpStage == 1) // Allows double jump
        {
            jumpStage++;
            return true;
        }
        return false;
    }


    private float coyoteTime = 0.2f; // Time allowed to jump after falling
    private float coyoteTimeCounter; // Tracks coyote time

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);

        if (raycastHit.collider != null)
        {
            // Check for OneWayPlatform rules
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform") &&
                (transform.position.y > raycastHit.collider.bounds.center.y))
            {
                coyoteTimeCounter = coyoteTime; // Reset coyote time
                jumpStage = 1; // Ensure player gets normal jump reset
                return true;
            }
        }

        bool isTouchingGround = raycastHit.collider != null;

        if (isTouchingGround)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when on solid ground
            jumpStage = 1; // Resext jump counter when touching ground
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time if not grounded

            // **NEW FIX:** If the player falls off a ledge, reset jumpStage to allow air jump
            if (jumpStage == 0)
            {
                jumpStage = 1; // Ensures they have an air jump available after falling
            }
        }

        return isTouchingGround;
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
    //follower mechanic
    public void AddFollower(GameObject followerObject)
    {
        Follower newFollower = followerObject.GetComponent<Follower>();
        if (followers.Count == 0)
        {
            newFollower.SetTarget(this.transform);
        }
        else
        {
            newFollower.SetTarget(followers[followers.Count - 1].transform);
        }
        followers.Add(newFollower);
    }
    public bool HasFollowers()//check for death save
    {
        return followers.Count > 0;
    }

    public void TriggerFollowerSave(Vector3 damagePosition)
    {
        if (followers.Count > 0)
        {
            StartCoroutine(FollowerSaveSequence(damagePosition));
        }
    }
    private IEnumerator FollowerSaveSequence(Vector3 damagePosition)
    {
        Time.timeScale = 0; // Pause the game
        Follower topFollower = followers[0];

        // Change the sorting order to bring the follower to the front
        SpriteRenderer followerRenderer = topFollower.GetComponent<SpriteRenderer>();
        if (followerRenderer != null)
        {
            followerRenderer.sortingOrder = 10; // Set this to a value higher than the player's sorting order
        }

        // Dash to the player's damage position
        topFollower.transform.position = damagePosition;

        // Play follower dying animation (assuming you have an animator and animation for this)
        Animator followerAnimator = topFollower.GetComponent<Animator>();
        if (followerAnimator != null)
        {
            followerAnimator.SetTrigger("die");
            // Wait until the follower is in the die animation state
            while (!followerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                yield return null; // Wait for the next frame
            }

            // Wait for the animation to finish using unscaled time
            float animationTime = followerAnimator.GetCurrentAnimatorStateInfo(0).length;
            float elapsedTime = 0f;
            while (elapsedTime < animationTime)
            {
                followerAnimator.Update(Time.unscaledDeltaTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        else
        {
            Debug.Log("Follower Animator not found");
        }

        // Heal the player
        GetComponent<PlayerStats>().Heal(100);

        // Remove the follower from the list and destroy the game object
        followers.RemoveAt(0);
        Destroy(topFollower.gameObject);
        UpdateFollowerTargets();

        Time.timeScale = 1; // Resume the game
    }

    private void UpdateFollowerTargets()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (i == 0)
            {
                followers[i].SetTarget(this.transform);
            }
            else
            {
                followers[i].SetTarget(followers[i - 1].transform);
            }
        }
    }

}
