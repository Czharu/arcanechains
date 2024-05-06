using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5f; // Speed of the laser
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
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        // Move the laser
        //Debug.Log("Moving laser at speed: " + speed + " with direction: " + direction);
        transform.position += direction * speed * Time.deltaTime;
    }
    public void Initialize(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized; // Set the direction based on target
        //Debug.Log("Direction set: " + direction);  // Log to verify the direction
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
}
