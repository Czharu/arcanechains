using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private Rigidbody2D parentRb;
    private Animator anim; // Reference to the Animator component
    [SerializeField] private float projectileDelay = 0.5f; // Delay before firing the projectile
    [SerializeField] private float projectileGravity = 1f; // Gravity scale for the projectile
    [SerializeField] private float projectileForwardVelocity = 5f; // Forward velocity for the projectile
    [SerializeField] private bool flipProjectile180 = false; // Option to flip the projectile 180 degrees upon spawning



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
        // Stop the enemy's movement alternative
        // parentRb.velocity = Vector2.zero;
        // Stop the enemy's horizontal movement
        parentRb.velocity = new Vector2(0, parentRb.velocity.y);

        // Reset the enemy's rotational velocity
        parentRb.angularVelocity = 0;

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

    public void EndStandAttack()//not needed yet
    {
        // Disable root motion after the standing attack
        //anim.applyRootMotion = false;
    }

    public void RangedAttack(GameObject projectilePrefab, Transform projectileSpawnPoint)
    {
        StartCoroutine(RangedAttackCoroutine(projectilePrefab, projectileSpawnPoint));
    }
    private IEnumerator RangedAttackCoroutine(GameObject projectilePrefab, Transform projectileSpawnPoint)
    {
        // Play the firing animation (if any)
        anim.SetTrigger("FireProjectile");

        // Instantiate the projectile at the specified position and attach it to the weapon initially
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.transform.parent = projectileSpawnPoint;

        // Adjust the initial rotation if needed
        if (flipProjectile180)
        {
            projectile.transform.Rotate(0, 0, 180);
        }

        // Get the projectile's Rigidbody2D component
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            // **Set the Rigidbody2D to kinematic mode initially**
            projectileRb.isKinematic = true;
        }

        // Wait for the specified delay before firing the projectile
        yield return new WaitForSeconds(projectileDelay);

        // Detach the projectile from the spawn point
        projectile.transform.parent = null;

        // Apply any additional logic for the projectile (e.g., setting velocity, enabling physics, etc.)

        if (projectileRb != null)
        {
            // **Set the Rigidbody2D to dynamic mode to apply gravity**
            projectileRb.isKinematic = false;
            // Set gravity scale
            projectileRb.gravityScale = projectileGravity;

            // Set initial forward velocity
            float direction = transform.localScale.x > 0 ? 1 : -1; // Determine the direction based on the scale
            projectileRb.velocity = projectile.transform.right * projectileForwardVelocity;
            // Set initial velocity or force here
            // Example: projectileRb.velocity = new Vector2(speed, 0);
        }

    }

    private bool IsGrounded()//trigger box collider to check if grounded for jump
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, LayerMask.GetMask("Ground"));
    }

}
