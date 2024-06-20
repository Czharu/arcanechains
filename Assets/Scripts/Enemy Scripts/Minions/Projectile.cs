using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] public bool flip180; // Should the projectile be flipped 180 degrees?
    public float damage; // Damage value for the projectile
    [SerializeField] public LayerMask groundLayer; // Layer mask for the ground
    [SerializeField] public bool impaleOnCollision = true; // Toggle to determine behavior on collision
    [SerializeField] private string poolTag;
    public bool isImpaled = false; // Flag to indicate if the projectile is impaled

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RotateToFaceTrajectory();
    }

    private void RotateToFaceTrajectory()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            if (flip180)
            {
                angle += 180; // Adjust angle if the projectile should be flipped
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//function to deal damage
    {
        // Check if the projectile hits the player
        if (collision.CompareTag("Player"))
        {
            CharacterStats playerStats = collision.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                if (impaleOnCollision) // Check the toggle to determine behavior
                {
                    ImpaleProjectile(collision.transform); // Impale the projectile on the player
                }
                else
                {
                    ReturnToPool(); // Deactivate the projectile after dealing damage
                    //Debug.Log(poolTag);
                }
            }
        }
        else if (IsGroundLayer(collision.gameObject))
        {
            if (impaleOnCollision) // Check the toggle to determine behavior
            {
                ImpaleProjectile(collision.transform); // Impale the projectile on the ground
            }
            else
            {
                ReturnToPool(); // Deactivate the projectile if it hits the ground
                //Debug.Log(poolTag);
            }
        }
    }

    private bool IsGroundLayer(GameObject obj)
    {
        // Check if the object is in the ground layer
        return (groundLayer.value & (1 << obj.layer)) > 0;
    }

    private void ImpaleProjectile(Transform target)
    {
        // Make the projectile a child of the target object
        transform.parent = target;

        // Disable Rigidbody2D to stop any further physics interactions
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        // Optionally, disable the collider to stop any further collisions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        isImpaled = true; // Flag to indicate if the projectile is impaled

        // Optionally, disable any scripts or components that should no longer be active

        // (e.g., you might want to disable this script itself)
        //this.enabled = false;
    }

    public void ResetProjectile()
    {
        // Reset any properties if necessary
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
        this.enabled = true;
        transform.parent = null; // Ensure the projectile is detached from any previous parent
        isImpaled = false; // Reset the impaled flag 
    }

    public void SetPoolTag(string tag)
    {
        poolTag = tag;
    }

    private void ReturnToPool()
    {
        if (string.IsNullOrEmpty(poolTag))
        {
            Debug.LogWarning("Pool tag is not set for projectile.");
            //Debug.Log(poolTag);
            return;
        }
        ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
    }
}
