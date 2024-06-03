using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed; // Public to set default in Inspector and modify dynamically
    private Vector3 direction; // Direction towards the player
    public LayerMask collisionMask; // Layers the laser can collide with
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        // Optional: Destroy the laser after a certain time to ensure it doesn't exist forever if it never hits anything
        Destroy(gameObject, 20f);
    }
    private void Update()
    {
        // Move the laser
        //Debug.Log("Moving laser at speed: " + speed + " with direction: " + direction);
        transform.position += direction * speed * Time.deltaTime;
    }
    // New Initialize method with an optional parameter
    public void Initialize(Vector3 targetPosition, float customSpeed, bool isDirectional = false)
    {
        speed = customSpeed; // Set the speed as provided
        if (isDirectional)
        {
            // If it's directional, the targetPosition argument is actually the direction vector
            direction = targetPosition.normalized;
        }
        else
        {
            // Calculate direction to the target position
            direction = (targetPosition - transform.position).normalized;
        }
    }
    public void DisableAnimator()
    {
        if (animator != null)
        {
            animator.enabled = false; // Disable the animator
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
        {
            Debug.Log("Laser collided with: " + other.gameObject.name);

            // Check if the collided object is the player
            if (other.gameObject.CompareTag("Player"))
            {
                // Access the CharacterStats script on the player and apply damage
                PlayerInteraction playerStats = other.gameObject.GetComponent<PlayerInteraction>();
                if (playerStats != null)
                {
                    playerStats.Damage(10); // Assume 10 is the damage value
                }
            }

            // Destroy the laser whether it hits the player or not
            Destroy(gameObject);
        }
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed; // Update the speed at which the laser will move.
        if (speed > 0 && direction != Vector3.zero)  // Make sure there is a direction set when setting speed.
        {
            direction.Normalize();
            direction *= speed;  // Apply the new speed to the direction
        }
    }
}
