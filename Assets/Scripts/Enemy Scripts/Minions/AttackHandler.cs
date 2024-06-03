using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim; // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Initialize the Animator component
    }

    public void JumpAttack(Vector2 targetPosition, int jumpHeight, float jumpStrength)
    {
        float distanceFromPlayer = targetPosition.x - transform.position.x;

        if (IsGrounded())
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Reset horizontal velocity
            float horizontalForce = distanceFromPlayer > 0 ? jumpStrength : -jumpStrength; // Determine the direction of the horizontal force
            rb.AddForce(new Vector2(horizontalForce, jumpHeight), ForceMode2D.Impulse); // Apply the jump force
            anim.SetTrigger("JumpAttack"); // Trigger the JumpAttack animation
        }
    }
    public void StandAttack()
    {
        // Stop the enemy's movement
        rb.velocity = Vector2.zero;

        // Trigger the StandingAttack animation in the enemy
        anim.SetTrigger("StandingAttack");

        // Trigger the StandingAttack animation in the Weapon child object if it exists
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            Animator weaponAnim = weapon.GetComponent<Animator>();
            if (weaponAnim != null)
            {
                weaponAnim.SetTrigger("StandingAttack");
            }
        }
    }

    private bool IsGrounded()//trigger box collider to check if grounded for jump
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, LayerMask.GetMask("Ground"));
    }
}
