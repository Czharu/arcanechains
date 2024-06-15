using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private Rigidbody2D parentRb;
    private Animator anim; // Reference to the Animator component


    void Start()
    {
        parentRb = GetComponentInParent<Rigidbody2D>(); // Get the Rigidbody2D component from the parent
        anim = GetComponent<Animator>(); // Initialize the Animator component

    }


    public void JumpAttack(Vector2 targetPosition, int jumpHeight, float jumpStrength)
    {
        float distanceFromPlayer = targetPosition.x - transform.position.x;

        if (IsGrounded())
        {
            parentRb.velocity = new Vector2(0, parentRb.velocity.y); // Reset horizontal velocity
            float horizontalForce = distanceFromPlayer > 0 ? jumpStrength : -jumpStrength; // Determine the direction of the horizontal force
            parentRb.AddForce(new Vector2(horizontalForce, jumpHeight), ForceMode2D.Impulse); // Apply the jump force
            anim.SetTrigger("JumpAttack"); // Trigger the JumpAttack animation
        }
    }
    public void StandAttack()
    {
        // Stop the enemy's movement
        parentRb.velocity = Vector2.zero;

        // Trigger the StandingAttack animation in the enemy
        anim.SetTrigger("StandingAttack");
        //anim.applyRootMotion = true;

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

    public void EndStandAttack()
    {
        // Disable root motion after the standing attack
        //anim.applyRootMotion = false;
    }

    public void RangedAttack(GameObject projectilePrefab, Vector2 spawnPosition)
    {
        // Play the firing animation (if any)
        anim.SetTrigger("FireProjectile");

        // Instantiate the projectile at the specified position
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Additional setup for the projectile can be done here (e.g., setting velocity, damage, etc.)
    }

    private bool IsGrounded()//trigger box collider to check if grounded for jump
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, LayerMask.GetMask("Ground"));
    }

}
