using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private bool flip180; // Should the projectile be flipped 180 degrees?
    public float damage; // Damage value for the projectile
    [SerializeField] private LayerMask groundLayer; // Layer mask for the ground

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits the player
        if (collision.CompareTag("Player"))
        {
            CharacterStats playerStats = collision.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Destroy(gameObject); // Destroy the projectile after dealing damage
            }
        }
        else if (IsGroundLayer(collision.gameObject))
        {
            Destroy(gameObject); // Destroy the projectile if it hits the ground
        }
    }
    private bool IsGroundLayer(GameObject obj)
    {
        // Check if the object is in the ground layer
        return (groundLayer.value & (1 << obj.layer)) > 0;
    }
}
