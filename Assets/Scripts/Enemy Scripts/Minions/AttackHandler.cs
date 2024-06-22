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
    [SerializeField] private GameObject standAttackHitbox; // Reference to the hitbox GameObject
    [SerializeField] private float standAttackDamageMulti = 1f; // Damage multiplier for stand attack
    [SerializeField] private float standAttackTime = 0.5f; // Time before the hitbox is disabled if no damage is dealt
    private CharacterStats enemyStats; // Reference to the enemy's stats
    public float StandAttackDamageMulti { get { return standAttackDamageMulti; } } // Public property to access the multiplier


    void Start()
    {
        parentRb = GetComponentInParent<Rigidbody2D>(); // Get the Rigidbody2D component from the parent
        anim = GetComponent<Animator>(); // Initialize the Animator component
        enemyStats = GetComponent<CharacterStats>(); // Initialize the enemy stats component
        // Find the StandAttackHitbox child GameObject
        //standAttackHitbox = transform.Find("StandAttackHitbox")?.gameObject; this has a performance hit instead of assigning it in inspector
        if (standAttackHitbox != null)
        {
            standAttackHitbox.SetActive(false); // Ensure hitbox is initially disabled
            standAttackHitbox.GetComponent<Collider2D>().isTrigger = true; // Ensure it's a trigger collider
        }

    }

    private bool HasParameter(string paramName, Animator animator)// to get rid of animator parameter caution events
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }

    public void JumpAttack(Vector2 targetPosition, int jumpHeight, float jumpStrength)
    {
        float distanceFromPlayer = targetPosition.x - transform.position.x;

        if (IsGrounded())
        {
            parentRb.velocity = new Vector2(0, parentRb.velocity.y); // Reset horizontal velocity
            float horizontalForce = distanceFromPlayer > 0 ? jumpStrength : -jumpStrength; // Determine the direction of the horizontal force
            parentRb.AddForce(new Vector2(horizontalForce, jumpHeight), ForceMode2D.Impulse); // Apply the jump force
            if (HasParameter("JumpAttack", anim))
            {
                anim.SetTrigger("JumpAttack"); // Trigger the JumpAttack animation
            }
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
        if (HasParameter("StandingAttack", anim))
        {
            anim.SetTrigger("StandingAttack");
        }
        //anim.applyRootMotion = true;

        // Trigger the StandingAttack animation in the Weapon child object if it exists
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            Animator weaponAnim = weapon.GetComponent<Animator>();
            if (weaponAnim != null)
            {
                if (HasParameter("StandingAttack", weaponAnim))
                {
                    weaponAnim.SetTrigger("StandingAttack");
                }
            }
        }
    }

    public void AnimStandAttack()//called from an animation clip event that finds the child "StandAttackHitbox" and enables it so it can trigger damage
    {
        //Debug.Log("connect");
        Collider2D hitboxCollider = standAttackHitbox.GetComponent<Collider2D>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
            StartCoroutine(ReEnableCollider(hitboxCollider));
        }
        if (standAttackHitbox != null)
        {
            standAttackHitbox.SetActive(true);
            StartCoroutine(DisableHitboxAfterTime(standAttackTime));
        }
    }
    private IEnumerator ReEnableCollider(Collider2D collider)// this is needed to fix a bug where if the player and the hitbox collider have not moved the player only takes one instance of damage and ignores future ones
    {
        // Wait for a frame to ensure the collider is re-enabled in the next physics update
        yield return null;
        collider.enabled = true;
    }

    private IEnumerator DisableHitboxAfterTime(float time)//disable the damage hitbox after Time, can be changed to be called from animation lip if needed
    {
        yield return new WaitForSeconds(time);
        if (standAttackHitbox != null)
        {
            standAttackHitbox.SetActive(false);
        }
    }

    public void EndStandAttack()//not needed yet, possible combo attacks later
    {
        // Disable root motion after the standing attack
        //anim.applyRootMotion = false;
    }

    public void RangedAttack(GameObject projectilePrefab, Transform projectileSpawnPoint, bool facingRight)
    {
        StartCoroutine(RangedAttackCoroutine(projectilePrefab, projectileSpawnPoint, facingRight));
    }

    private IEnumerator RangedAttackCoroutine(GameObject projectilePrefab, Transform projectileSpawnPoint, bool facingRight)
    {
        // Play the firing animation (if any)
        if (HasParameter("FireProjectile", anim))
        {
            anim.SetTrigger("FireProjectile");
        }

        // Instantiate the projectile at the specified position and attach it to the weapon initially
        GameObject projectile = ObjectPooler.Instance.SpawnFromPool("Projectile", projectileSpawnPoint.position, projectileSpawnPoint.rotation, projectilePrefab);


        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetPoolTag("Projectile"); // Set the pool tag for the projectile
        }
        projectile.transform.parent = projectileSpawnPoint; // Reattach to the spawn point

        // Adjust the initial rotation if needed
        if (flipProjectile180)
        {
            projectile.transform.Rotate(0, 0, 180);
        }

        // Get the projectile's Rigidbody2D component
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            // Set the Rigidbody2D to kinematic mode initially
            projectileRb.isKinematic = true;
        }

        // Wait for the specified delay before firing the projectile
        yield return new WaitForSeconds(projectileDelay);

        // Set the damage value on the projectile

        if (projScript != null)
        {
            projScript.ResetProjectile();
            projScript.damage = enemyStats.damage.GetValue();
            projScript.SetPoolTag("Projectile"); // Set the pool tag for the projectile
        }

        // Detach the projectile from the spawn point
        projectile.transform.parent = null;

        // Apply any additional logic for the projectile (e.g., setting velocity, enabling physics, etc.)
        // Flip the Y-scale if the enemy is facing left
        if (facingRight)// banna not comming out like how it is gripped without this
        {
            Vector3 scale = projectile.transform.localScale;
            scale.y *= -1;
            projectile.transform.localScale = scale;
        }

        if (projectileRb != null)
        {
            // Set the Rigidbody2D to dynamic mode to apply gravity
            projectileRb.isKinematic = false;
            // Set gravity scale
            projectileRb.gravityScale = projectileGravity;

            // Set initial forward velocity
            float direction = transform.localScale.x > 0 ? 1 : -1; // Determine the direction based on the scale
            projectileRb.velocity = projectile.transform.right * projectileForwardVelocity;
        }
    }

    private bool IsGrounded()//trigger box collider to check if grounded for jump
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .2f, LayerMask.GetMask("Ground"));
    }
}
